namespace Cross.CQRS.Licensing;

internal class License
{
    internal License(params Claim[] claims) : this(new ClaimsPrincipal(new ClaimsIdentity(claims)))
    {

    }

    public License(ClaimsPrincipal claims)
    {
        if (Guid.TryParse(claims.FindFirst("account_id")?.Value, out var accountId))
        {
            AccountId = accountId;
        }

        CustomerId = claims.FindFirst("customer_id")?.Value;
        SubscriptionId = claims.FindFirst("sub_id")?.Value;

        if (long.TryParse(claims.FindFirst("iat")?.Value, out var iat))
        {
            var startedAt = DateTimeOffset.FromUnixTimeSeconds(iat);
            StartDate = startedAt;
        }

        if (long.TryParse(claims.FindFirst("exp")?.Value, out var exp))
        {
            var expiredAt = DateTimeOffset.FromUnixTimeSeconds(exp);
            ExpirationDate = expiredAt;
        }

        if (Enum.TryParse<EditionEnum>(claims.FindFirst("edition")?.Value, out var edition))
        {
            Edition = edition;
        }

        if (Enum.TryParse<ProductTypeEnum>(claims.FindFirst("type")?.Value, out var productType))
        {
            ProductType = productType;
        }

        IsConfigured = AccountId != null
                       && CustomerId != null
                       && SubscriptionId != null
                       && StartDate != null
                       && ExpirationDate != null
                       && Edition != null
                       && ProductType != null;
    }

    public Guid? AccountId { get; }
    public string? CustomerId { get; }
    public string? SubscriptionId { get; }
    public DateTimeOffset? StartDate { get; }
    public DateTimeOffset? ExpirationDate { get; }
    public EditionEnum? Edition { get; }
    public ProductTypeEnum? ProductType { get; }

    public bool IsConfigured { get; }
}