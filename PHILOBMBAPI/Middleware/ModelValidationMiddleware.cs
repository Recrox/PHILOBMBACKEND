using Microsoft.AspNetCore.Mvc.ModelBinding;

public class ModelValidationMiddleware
{
    private readonly RequestDelegate _next;

    public ModelValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Vérifier si la requête est de type POST ou PUT
        if (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put)
        {
            // Vérifier si le ModelState est présent dans le HttpContext
            if (context.Items.TryGetValue("ModelState", out var modelStateObj) 
                && modelStateObj is ModelStateDictionary modelState)
            {
                // Vérifier si le ModelState est valide
                if (!modelState.IsValid)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest; // 400 Bad Request
                    await context.Response.WriteAsJsonAsync(modelState); // Renvoyer le ModelState en tant que JSON
                    return;
                }
            }
        }

        // Appeler le prochain middleware dans la pipeline
        await _next(context);
    }
}
