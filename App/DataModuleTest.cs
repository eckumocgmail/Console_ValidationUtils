using Console_Validation;

using DataADO;

using System.Collections.Generic;

internal class DataModuleTest : TestingUnit
{
    public DataModuleTest()
    {
        Push(new ADODbConnectorsTest());
        Push(new ADOExecutorTest());
        Push(new ADODbMetadataTest());
        Push(new ADODbMigBuilderTest());
        Push(new EntityRepositoryTest());
        Push(new AuthorizationDataTest());
        Push(new BusinessDataTest());

    }

    public override void OnTest()
    {
             
    }
}
