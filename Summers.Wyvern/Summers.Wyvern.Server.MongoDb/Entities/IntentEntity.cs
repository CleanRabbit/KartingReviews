namespace Summers.Wyvern.Server.MongoDb.Entities
{
    public class IntentEntity : EntityBase
    {
        public string Utterance { get; set; }
        public string Intent { get; set; }
    }
}