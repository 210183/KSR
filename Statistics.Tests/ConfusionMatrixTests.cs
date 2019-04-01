using Core.Models;
using Core.Models.Concrete;
using FluentAssertions;
using NUnit.Framework;
using Statistics.Calculation;
using Statistics.Models;
using System.Collections.Generic;

namespace Statistics.Tests
{
    public class ConfusionMatrixTests
    {
        private ConfusionMatrix _expected;

        [Test]
        public void Single_CorrectlyAssigned()
        {
            var classified = new List<ClassifiedDataSampleMock>
            {
                new ClassifiedDataSampleMock
                {
                    Labels = new LabelsCollection(new Label[] {new Label("Helm's Deep")}),
                    AssignedLabels = new LabelsCollection(new Label[]{new Label("Helm's Deep")})
                },
            };

            _expected = MatrixSource.GetSingleCorrectlyAssigned(new List<string>{ "Helm's Deep"});

            Then(classified);
        }

        [Test]
        public void Two_IncorrectlyAssigned()
        {
            var classified = new List<ClassifiedDataSampleMock>
            {
                new ClassifiedDataSampleMock
                {
                    Labels = new LabelsCollection(new Label[] {new Label("Helm's Deep")}),
                    AssignedLabels = new LabelsCollection(new Label[]{new Label("Minas Tirith")})
                },
                new ClassifiedDataSampleMock
                {
                    Labels = new LabelsCollection(new Label[] {new Label("Minas Tirith") }),
                    AssignedLabels = new LabelsCollection(new Label[]{new Label("Helm's Deep") })
                },
            };

            _expected = MatrixSource.GetTwoIncorrectlyAssigned(new List<string> { "Helm's Deep", "Minas Tirith" });

            Then(classified);
        }

        [Test]
        public void Two_OneIncorrectlyAssigned()
        {
            var classified = new List<ClassifiedDataSampleMock>
            {
                new ClassifiedDataSampleMock
                {
                    Labels = new LabelsCollection(new Label[] {new Label("Helm's Deep")}),
                    AssignedLabels = new LabelsCollection(new Label[]{new Label("Helm's Deep") })
                },
                new ClassifiedDataSampleMock
                {
                    Labels = new LabelsCollection(new Label[] {new Label("Minas Tirith") }),
                    AssignedLabels = new LabelsCollection(new Label[]{new Label("Helm's Deep") })
                },
            };

            _expected = MatrixSource.GetTwoOneIncorrectlyAssigned(new List<string> { "Helm's Deep", "Minas Tirith" });

            Then(classified);
        }

        public void Then(IReadOnlyList<IClassifiedDataSample> samples)
        {
            ConfusionMatrix result = Calculator.CalculateConfusionMatrix(samples);
            result.Should().BeEquivalentTo(_expected);
        }


        internal class ClassifiedDataSampleMock : IClassifiedDataSample
        {
            public OrderedAttributes Attributes { get; }
            public LabelsCollection Labels { get; set; }
            public LabelsCollection AssignedLabels { get; set; }
        }
    }
}