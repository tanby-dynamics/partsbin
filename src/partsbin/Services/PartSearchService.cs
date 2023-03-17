using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using partsbin.Models;

namespace partsbin.Services;

public interface IPartSearchService
{
    IEnumerable<int> Search(string phrase);
    void IndexPart(Part part);
    void RemovePart(Part part);
    void ClearIndex();

}

public class PartSearchService : IPartSearchService
{
    private readonly ISearchFactory _searchFactory;
    private const int SearchHitLimit = 1000;

    public PartSearchService(ISearchFactory searchFactory)
    {
        _searchFactory = searchFactory;
    }

    private static IEnumerable<(string FieldName, Func<Part, string> ValueAccessor)> GetIndexFields()
    {
        yield return ("PartType", x => x.PartType ?? string.Empty);
        yield return ("PartType", x => x.PartType ?? string.Empty);
        yield return ("Range", x => x.Range ?? string.Empty);
        yield return ("Location", x => x.Location ?? string.Empty);
        yield return ("Manufacturer", x => x.Manufacturer ?? string.Empty);
        yield return ("HtmlNotes", x => x.HtmlNotes ?? string.Empty);
        // TODO documents
        // TODO suppliers
        yield return ("PackageType", x => x.PackageType ?? string.Empty);
        yield return ("FormattedValue", x => x.FormattedValue);
        yield return ("PartName", x => x.PartName ?? string.Empty);
        yield return ("PartNumber", x => x.PartNumber ?? string.Empty);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="phrase"></param>
    /// <returns>A list of part ids that match the search phrase</returns>
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
        var booleanQuery = new BooleanQuery();
        foreach (var query in fuzzyQueries)
        {
            booleanQuery.Add(query, Occur.SHOULD);
        }
        foreach (var query in wildcardQueries)
        {
            booleanQuery.Add(query, Occur.SHOULD);
        }
        
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

    public void IndexPart(Part part)
    {
        var document = new Document
        {
            new Int64Field("Id", part.Id, Field.Store.YES)
        };
        var indexFields = GetIndexFields()
            .Select(field => new StringField(
                field.FieldName,
                field.ValueAccessor(part).ToLower(),
                Field.Store.YES));
        
        foreach (var field in indexFields)
        {
            document.Add(field);
        }
        
        using var dir = _searchFactory.GetDirectory();
        using var writer = _searchFactory.GetWriter(dir);
        writer.AddDocument(document);
        writer.Flush(true, true);
    }

    public void RemovePart(Part part)
    {
        using var dir = _searchFactory.GetDirectory();
        using var writer = _searchFactory.GetWriter(dir);

        var term = new Term("Id", part.Id.ToString());
        
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