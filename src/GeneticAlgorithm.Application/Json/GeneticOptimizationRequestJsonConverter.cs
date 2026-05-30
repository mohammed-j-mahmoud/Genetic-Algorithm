using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using GeneticAlgorithm.Application.Models;

namespace GeneticAlgorithm.Application.Json
{
    /// <summary>
    /// Accepts optimization JSON either nested ({ "simulation": { ... } }) or flat
    /// (economics at the root with maxTrucks/maxLoaders/maxScalers).
    /// truckCount/loaderCount/scalerCount are optional on search routes and ignored by optimizers.
    /// </summary>
    public sealed class GeneticOptimizationRequestJsonConverter : JsonConverter<GeneticOptimizationRequest>
    {
        private static readonly HashSet<string> SimulationPropertyNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            nameof(SimulationRequest.CoalVolume),
            nameof(SimulationRequest.TruckCount),
            nameof(SimulationRequest.TruckLoadVolume),
            nameof(SimulationRequest.TruckCostPerDay),
            nameof(SimulationRequest.LoaderCount),
            nameof(SimulationRequest.LoaderCostPerDay),
            nameof(SimulationRequest.ScalerCount),
            nameof(SimulationRequest.ScalerCostPerDay),
            nameof(SimulationRequest.ProjectDurationDays),
            nameof(SimulationRequest.DelayCostPerDay),
            nameof(SimulationRequest.LoadingDistribution),
            nameof(SimulationRequest.WeighingDistribution),
            nameof(SimulationRequest.TravelingDistribution)
        };

        public override GeneticOptimizationRequest Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            using JsonDocument document = JsonDocument.ParseValue(ref reader);
            JsonElement root = document.RootElement;

            if (root.ValueKind != JsonValueKind.Object)
                throw new JsonException("Optimization request must be a JSON object.");

            JsonElement source = root;
            if (!root.TryGetProperty("simulation", out _))
            {
                using JsonDocument wrapped = WrapFlatSimulationProperties(root);
                source = wrapped.RootElement.Clone();
            }

            GeneticOptimizationRequest request = new GeneticOptimizationRequest();
            BindOptimizationProperties(source, request);

            if (!source.TryGetProperty("simulation", out JsonElement simulationElement)
                || simulationElement.ValueKind == JsonValueKind.Null)
            {
                return request;
            }

            request.Simulation = JsonSerializer.Deserialize<SimulationRequest>(
                simulationElement.GetRawText(),
                options);

            return request;
        }

        public override void Write(Utf8JsonWriter writer, GeneticOptimizationRequest value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartObject();
            writer.WriteNumber("populationSize", value.PopulationSize);
            writer.WriteNumber("maxTrucks", value.MaxTrucks);
            writer.WriteNumber("maxLoaders", value.MaxLoaders);
            writer.WriteNumber("maxScalers", value.MaxScalers);
            writer.WriteNumber("generations", value.Generations);
            writer.WriteNumber("mutationRate", value.MutationRate);
            writer.WritePropertyName("simulation");
            JsonSerializer.Serialize(writer, value.Simulation, options);
            writer.WriteEndObject();
        }

        private static void BindOptimizationProperties(JsonElement root, GeneticOptimizationRequest request)
        {
            if (TryReadInt(root, "populationSize", out int populationSize))
                request.PopulationSize = populationSize;
            if (TryReadInt(root, "maxTrucks", out int maxTrucks))
                request.MaxTrucks = maxTrucks;
            if (TryReadInt(root, "maxLoaders", out int maxLoaders))
                request.MaxLoaders = maxLoaders;
            if (TryReadInt(root, "maxScalers", out int maxScalers))
                request.MaxScalers = maxScalers;
            if (TryReadInt(root, "generations", out int generations))
                request.Generations = generations;
            if (TryReadDouble(root, "mutationRate", out double mutationRate))
                request.MutationRate = mutationRate;
        }

        private static bool TryReadInt(JsonElement root, string propertyName, out int value)
        {
            value = 0;
            if (!root.TryGetProperty(propertyName, out JsonElement element))
                return false;

            value = element.GetInt32();
            return true;
        }

        private static bool TryReadDouble(JsonElement root, string propertyName, out double value)
        {
            value = 0;
            if (!root.TryGetProperty(propertyName, out JsonElement element))
                return false;

            value = element.GetDouble();
            return true;
        }

        private static JsonDocument WrapFlatSimulationProperties(JsonElement root)
        {
            using var stream = new System.IO.MemoryStream();
            using (var jsonWriter = new Utf8JsonWriter(stream))
            {
                jsonWriter.WriteStartObject();

                using (JsonObjectWriter simulationWriter = new JsonObjectWriter(jsonWriter, "simulation"))
                {
                    foreach (JsonProperty property in root.EnumerateObject())
                    {
                        if (SimulationPropertyNames.Contains(property.Name))
                            simulationWriter.WriteProperty(property);
                        else
                        {
                            jsonWriter.WritePropertyName(property.Name);
                            property.Value.WriteTo(jsonWriter);
                        }
                    }
                }

                jsonWriter.WriteEndObject();
            }

            stream.Position = 0;
            return JsonDocument.Parse(stream);
        }

        private sealed class JsonObjectWriter : IDisposable
        {
            private readonly Utf8JsonWriter _writer;

            public JsonObjectWriter(Utf8JsonWriter writer, string objectName)
            {
                _writer = writer;
                _writer.WriteStartObject(objectName);
            }

            public void WriteProperty(JsonProperty property)
            {
                _writer.WritePropertyName(property.Name);
                property.Value.WriteTo(_writer);
            }

            public void Dispose() => _writer.WriteEndObject();
        }
    }
}
