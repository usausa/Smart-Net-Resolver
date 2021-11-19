namespace Smart.Resolver.Benchmark;

using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;

public class BenchmarkConfig : ManualConfig
{
    public BenchmarkConfig()
    {
        AddColumn(
            StatisticColumn.Mean,
            StatisticColumn.Min,
            StatisticColumn.Max,
            StatisticColumn.P90,
            StatisticColumn.Error,
            StatisticColumn.StdDev);
        AddDiagnoser(MemoryDiagnoser.Default);
        AddExporter(MarkdownExporter.Default, MarkdownExporter.GitHub);
        AddExporter(CsvExporter.Default);
        //AddExporter(CsvMeasurementsExporter.Default);
        //AddExporter(RPlotExporter.Default);
        AddJob(Job.MediumRun);
    }
}
