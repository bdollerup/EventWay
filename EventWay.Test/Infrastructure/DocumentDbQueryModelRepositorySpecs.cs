﻿using System;
using System.Threading.Tasks;
using EventWay.Core;
using EventWay.Infrastructure.CosmosDb;
using EventWay.Query;
using NUnit.Framework;

namespace EventWay.Test.Infrastructure
{
    [TestFixture(Category = "Integration")]
    public class DocumentDbQueryModelRepositorySpecs
    {
        private readonly string _database = "vanda-integration-test";
        private readonly string _collection = "Projections";
        private readonly string _endpoint = "https://localhost:8081";
        private readonly string _authKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        [Test]
        public void ShouldSuccesfullyInitializeDatabaseAndCollection()
        {
            // ARRANGE
            var repository = new DocumentDbQueryModelRepository(_database, _collection, _endpoint, _authKey);

            // ACT
            repository.Initialize();

            // ASSERT
        }

        [Test]
        public async Task ShouldSuccesfullyCreateAndHydrateQueryModel()
        {
            // ARRANGE
            var repository = new DocumentDbQueryModelRepository(_database, _collection, _endpoint, _authKey);

            var queryModelId = CombGuid.Generate();
            var testQueryModel = new TestQueryModel(queryModelId, "Hello Integration Test!");

            // ACT
            await repository.Save(testQueryModel);
            var hydratedQueryModel = await repository.GetById<TestQueryModel>(queryModelId);

            // ASSERT
            Assert.IsNotNull(hydratedQueryModel);
            Assert.AreEqual(queryModelId.ToString(), hydratedQueryModel.AggregateId);
            Assert.AreEqual("Hello Integration Test!", testQueryModel.DummyPayload);
            Assert.AreEqual(testQueryModel.DummyPayload, hydratedQueryModel.DummyPayload);
        }
    }

    public class TestQueryModel : QueryModel
    {
        public TestQueryModel(Guid aggregateId, string payload) : base(aggregateId)
        {
            DummyPayload = payload;
        }

        public string DummyPayload { get; set; }
    }
}