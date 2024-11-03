using MongoDB.Bson.Serialization;
using StudentPortalServer.Entities.Page;

namespace StudentPortalServer.Helpers;

public static class MongoDBSerializerHelper
{
    public static void Configure()
    {
        BsonClassMap.RegisterClassMap<ISPComponent>(cm =>
        {
            cm.AutoMap();
            cm.SetIsRootClass(true);
        });

        BsonClassMap.RegisterClassMap<MarkdownComponentData>(cm =>
        {
            cm.AutoMap();
            cm.SetDiscriminator("markdown");
        });

        BsonClassMap.RegisterClassMap<SectionComponentData>(cm =>
        {
            cm.AutoMap();
            cm.SetDiscriminator("section");
        });
    }
}