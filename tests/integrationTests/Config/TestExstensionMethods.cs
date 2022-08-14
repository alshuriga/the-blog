namespace MiniBlog.Tests.Config;    

public static class TestExstensionMethods
{
    public static void AppendFakeAuth(this HttpRequestMessage request, bool isAdmin = false)
    {
        request.Headers.Add("test-username", "testadmin");
        request.Headers.Add("test-email", "admin@example.com");
        request.Headers.Add("test-role", isAdmin ? "Admins" : "Normal");
        request.Headers.Add("X-Integration-Test-Auth-Header", "true");
    }
}