using Humanizer;

namespace StudentPortal.PageStorage.Services;

public class KeyService {
    public string GetSlug(string pageName) {
        string res = pageName.Dehumanize().Transform(To.LowerCase);

        return res;
    }
}