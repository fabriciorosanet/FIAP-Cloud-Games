namespace FCG.Api.Areas.Usuarios.Endpoints;

public static class UsuarioEndpointsMapper
{
    public static void Map(IEndpointRouteBuilder routes)
    {
        routes.MapUsuarioEndpoints();
    }
}
