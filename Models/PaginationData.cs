namespace MiniBlog.Models;

public class PaginationData
{
    public int CurrentPage { get; set; }
    public int EntriesPerPage { get; set; }
    public int GeneralCount { get; set; }
    public int PageNumber => (int)Math.Ceiling(GeneralCount / (float)EntriesPerPage);
    public int SkipNumber => CurrentPage <= 1 ? 0 : EntriesPerPage * (CurrentPage - 1);


    public static PaginationData? CreatePaginationDataOrNull(int currentEntry, int entriesPerPage, int generalCount)
    {
        if(generalCount <= entriesPerPage) return null;
        return new PaginationData() {CurrentPage = currentEntry, EntriesPerPage = entriesPerPage, GeneralCount = generalCount };
    }
}