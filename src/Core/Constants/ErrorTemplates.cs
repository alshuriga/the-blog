namespace MiniBlog.Core.Constants;

public static class ErrorTemplates
{
    public static string StatusCodePageTemplate = @"
    <!DOCTYPE html>
<html>

<head>
    <title>MiniBlog</title>
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <link rel=""stylesheet"" href=""/lib/bootstrap/css/bootstrap.min.css"">
</head>

<body>
<div class=""navbar navbar-expand-lg navbar-dark bg-dark"">
    <div class=""container"">
    <a asp-action=""Index"" asp-controller=""Home"" class=""navbar-brand""><i class=""fa-solid fa-book me-2""></i>MiniBlog</a>
    </div>  
</div>

    <div class=""container my-5"">
    <div class=""row""><div class=""alert alert-danger text-center"">Error number {0}</div></div>
    
    </div>
</body>

</html>
    ";
}