using RealEstate.Core.Entities;

namespace RealEstate.Tests.EntityMock
{
    public static class PropertyTraceMockFactory
    {
        public static List<PropertyTrace> CreatePropertyTraces(long propertyId, int traceCount = 10)
        {
            if (traceCount <= 0)
                throw new ArgumentException("traceCount must be greater than zero.", nameof(traceCount));

            var traces = new List<PropertyTrace>();

            for (int i = 0; i < traceCount; i++)
            {
                var traceId = i + 1; // Assuming Ids start from 1

                var trace = new PropertyTrace
                {
                    Id = traceId,
                    DateSale = DateTime.Now.AddDays(-i), // Example date, with traces from recent to older
                    Name = $"Trace {traceId}",
                    Value = 50000m + (traceId * 1000), // Example value
                    Tax = 0.1m * (50000m + (traceId * 1000)), // Example tax calculation
                    IdProperty = propertyId // Assign the property ID
                };

                traces.Add(trace);
            }

            return traces;
        }
    }
}
