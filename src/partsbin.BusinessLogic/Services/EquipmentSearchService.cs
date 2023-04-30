using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using partsbin.BusinessLogic.Helpers;
using partsbin.BusinessLogic.Models;

namespace partsbin.BusinessLogic.Services;

public interface IEquipmentSearchService
{
    IEnumerable<int> Search(string phrase);
    void IndexEquipment(Equipment equipment);
    void RemoveEquipment(Equipment equipment);
    void ClearIndex();
}

public class EquipmentSearchService : IEquipmentSearchService
{
    private readonly ISearchFactory _searchFactory;
    private const int SearchHitLimit = 1000;

    public EquipmentSearchService(ISearchFactory searchFactory)
    {
        _searchFactory = searchFactory;
    }

    private static IEnumerable<(string FieldName, Func<Equipment, string> ValueAccessor)> GetIndexFields()
    {
        yield return ("EquipmentType", x => x.EquipmentType ?? string.Empty);
        yield return ("EquipmentName", x => x.EquipmentName ?? string.Empty);
        yield return ("Manufacturer", x => x.Manufacturer ?? string.Empty);
        yield return ("ModelNumber", x => x.ModelNumber ?? string.Empty);
        yield return ("Location", x => x.Location ?? string.Empty);
        yield return ("HtmlNotes", x => x.HtmlNotes ?? string.Empty);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="phrase"></param>
    /// <returns>A list of equipment ids that match the search phrase</returns>
    public IEnumerable<int> Search(string phrase)
    {
        phrase = phrase.ToLower();
        
        var fieldNames = GetIndexFields()
            .Select(x => x.FieldName)
            .ToArray();

        // Build up fuzzy queries
        var fuzzyQueries = fieldNames
            .Select(fieldName => new Term(fieldName, phrase))
            .Select(term => new FuzzyQuery(term));
        
        // Build up wildcard queries
        var wildcardKeywords = phrase.Split(' ')
            .Select(token => $"*{token}*");
        var wildcardQueries =
            from keyword in wildcardKeywords
            from fieldName in fieldNames
            select new Term(fieldName, keyword)
            into term
            select new WildcardQuery(term);

        // Combine into a single boolean query
        var combinedQuery = new BooleanQuery();        
        foreach (var query in fuzzyQueries)
        {
            combinedQuery.Add(query, Occur.SHOULD);
        }
        foreach (var query in wildcardQueries)
        {
            combinedQuery.Add(query, Occur.SHOULD);
        }
        var booleanQuery = new BooleanQuery();
        // limit to equipment
        var rootCollectionItemTypeTerm = new Term("RootCollectionItemType", RootCollectionItemType.Equipment.ToString());
        booleanQuery.Add(new TermQuery(rootCollectionItemTypeTerm), Occur.MUST);
        // add combined query
        booleanQuery.Add(combinedQuery, Occur.MUST);

        // Do the actual search and get a list of part IDs
        using var dir = _searchFactory.GetDirectory();
        using var reader = _searchFactory.GetReader(dir);
        var searcher = new IndexSearcher(reader);
        var hits = searcher.Search(booleanQuery, SearchHitLimit);
        var docIds = hits.ScoreDocs.Select(x => x.Doc);
        var docs = docIds.Select(docId => searcher.Doc(docId));
        var partIds = docs
            .Select(doc => int.Parse(doc.Get("Id")))
            .ToArray();

        return partIds;
    }

    public void IndexEquipment(Equipment equipment)
    {
        var document = new Document
        {
            new Int64Field("Id", equipment.Id, Field.Store.YES)
        };
        var indexFields = GetIndexFields()
            .Select(field => new StringField(
                field.FieldName,
                field.ValueAccessor(equipment).ToLower(),
                Field.Store.YES));
        
        foreach (var field in indexFields)
        {
            document.Add(field);
        }

        document.Add(new StringField("RootCollectionItemType", RootCollectionItemType.Equipment.ToString(), Field.Store.YES));
        
        using var dir = _searchFactory.GetDirectory();
        using var writer = _searchFactory.GetWriter(dir);
        writer.AddDocument(document);
        writer.Flush(true, true);
    }

    public void RemoveEquipment(Equipment equipment)
    {
        using var dir = _searchFactory.GetDirectory();
        using var writer = _searchFactory.GetWriter(dir);

        var term = new Term("Id", equipment.Id.ToString());
        
        writer.DeleteDocuments(term);
        writer.Commit();
    }

    public void ClearIndex()
    {
        using var dir = _searchFactory.GetDirectory();
        using var writer = _searchFactory.GetWriter(dir);
        
        writer.DeleteAll();
        writer.Commit();
    }
}