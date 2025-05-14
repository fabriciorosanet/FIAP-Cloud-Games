namespace FCG.Api.Areas.Auth;

public class AuthEndpointsMapper
{
    public static void Map(IEndpointRouteBuilder routes)
    {
        routes.MapAuth();
    }
}