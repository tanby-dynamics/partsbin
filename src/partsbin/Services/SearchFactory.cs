using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace partsbin.Services;

public interface ISearchFactory
{
    FSDirectory GetDirectory();
    IndexWriter GetWriter(FSDirectory fsDirectory);
    IndexReader GetReader(FSDirectory fsDirectory);
}

public class SearchFactory : ISearchFactory
{
    private readonly bool _isProduction;
    private const LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48; 

    public SearchFactory(bool isProduction)
    {
        _isProduction = isProduction;
    }

    private string IndexPath => _isProduction
        ? "/data/partsbin-lucene-index"
        : "./partsbin-lucene-index";
    
    /// <summary>
    /// The FSDirectory this returns needs to be disposed
    /// </summary>
    /// <returns></returns>
    public FSDirectory GetDirectory()
    {
        return FSDirectory.Open(IndexPath);
    }
    
    /// <summary>
    /// The writer needs to be disposed
    /// </summary>
    /// <param name="fsDirectory"></param>
    /// <returns></returns>
    public IndexWriter GetWriter(FSDirectory fsDirectory)
    {
        var analyzer = new StandardAnalyzer(AppLuceneVersion);
        var config = new IndexWriterConfig(AppLuceneVersion, analyzer);
        var writer = new IndexWriter(fsDirectory, config);

        return writer;
    }

    /// <summary>
    /// The reader needs to be disposed
    /// </summary>
    /// <param name="fsDirectory"></param>
    /// <returns></returns>
    public IndexReader GetReader(FSDirectory fsDirectory)
    {
        return DirectoryReader.Open(fsDirectory);
    }
}



