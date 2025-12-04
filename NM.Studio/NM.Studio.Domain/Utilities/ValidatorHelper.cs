using System.Collections;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NM.Studio.Domain.Shared.Exceptions;

namespace NM.Studio.Domain.Utilities
{
    public static class ValidatorHelper
    {
        // Store global service provider
        public static IServiceProvider? ServiceProvider { get; set; }

        public static async Task ValidateDynamicAsync(this object data)
        {
            if (ServiceProvider is null)
                throw new InvalidOperationException("ServiceProvider chưa được set. Thử gọi ValidatorHelper.ServiceProvider = app.Services trước.");

            if (data is null) return;

            if (data is IEnumerable list && data is not string)
            {
                foreach (var item in list)
                    await ValidateSingleAsync(item);
            }
            else
            {
                await ValidateSingleAsync(data);
            }
        }

        private static async Task ValidateSingleAsync(object item)
        {
            if (item is null || ServiceProvider is null) return;

            var validatorType = typeof(IValidator<>).MakeGenericType(item.GetType());
            dynamic? validator = ServiceProvider.GetService(validatorType);

            if (validator is null) return;

            var result = await validator.ValidateAsync((dynamic)item);

            if (!result.IsValid)
                throw new DomainException(result.Errors);
        }
    }
}