using System.Collections.Generic;
using Bogus;
using RedisStudio.DbContext;

namespace RedisStudio.Utility;

public static class DbUtility
{
    public static List<Travel> Feed(int count)
    {
        var faker = new Faker<Travel>()
            .RuleFor(t => t.DossierCode, f => f.Random.AlphaNumeric(10))
            .RuleFor(t => t.QAIncidentId, f => f.Random.Int(1000, 9999))
            .RuleFor(t => t.City, f => f.Address.City())
            .RuleFor(t => t.Nation, f => f.Address.Country())
            .RuleFor(t => t.Latitudine, f => f.Address.Latitude())
            .RuleFor(t => t.Longitudine, f => f.Address.Longitude())
            .RuleFor(t => t.StartDate, f => f.Date.Past())
            .RuleFor(t => t.EndDate, f => f.Date.Future())
            .RuleFor(t => t.TransportType, f => f.Random.Int(1, 3))
            .RuleFor(t => t.InsertDate, f => f.Date.Past())
            .RuleFor(t => t.InsertBy, f => f.Name.FullName())
            .RuleFor(t => t.ModifiedDate, f => f.Date.Past())
            .RuleFor(t => t.ModifiedBy, f => f.Name.FullName())
            .RuleFor(t => t.Enabled, f => f.Random.Bool());

        return faker.Generate(count);
    }
}

