using FluentValidation;
using PHILOBMCore.Models;

namespace PHILOBMBAPI.Validators;
public class CarValidator : AbstractValidator<Car>
{
    public CarValidator()
    {
        RuleFor(car => car.Brand)
            .NotEmpty().WithMessage("La marque est requise.");

        RuleFor(car => car.Model)
            .NotEmpty().WithMessage("Le modèle est requis.");

        RuleFor(car => car.LicensePlate)
            .NotEmpty().WithMessage("Le numéro de plaque est requis.");

        RuleFor(car => car.Mileage)
            .GreaterThanOrEqualTo(0).WithMessage("Le kilométrage doit être supérieur ou égal à zéro.")
            .LessThanOrEqualTo(2000000).WithMessage("Le kilométrage doit être inférieur ou égal à 2 000 000.");
    }
}

