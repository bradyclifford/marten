﻿using System.Linq;
using Marten.Linq;
using Marten.Services;
using Marten.Testing.Documents;
using Shouldly;
using Xunit;

namespace Marten.Testing.Linq
{
    public class select_transformations_Tests : DocumentSessionFixture<NulloIdentityMap>
    {
        [Fact]
        public void build_query_for_a_single_field()
        {
            theSession.Query<User>().Select(x => x.UserName).FirstOrDefault().ShouldBeNull();

            var cmd = theSession.Query<User>().Select(x => x.UserName).ToCommand(FetchType.FetchMany);

            cmd.CommandText.ShouldBe("select d.data ->> 'UserName' from public.mt_doc_user as d");
        }
    }

    public class select_transformations_with_database_schema_Tests : DocumentSessionFixture<NulloIdentityMap>
    {
        protected override void StoreOptions(StoreOptions options)
        {
            options.DatabaseSchemaName = "other";
        }

        [Fact]
        public void build_query_for_a_single_field()
        {
            theSession.Query<User>().Select(x => x.UserName).FirstOrDefault().ShouldBeNull();

            var cmd = theSession.Query<User>().Select(x => x.UserName).ToCommand(FetchType.FetchMany);

            cmd.CommandText.ShouldBe("select d.data ->> 'UserName' from other.mt_doc_user as d");
        }
    }
}