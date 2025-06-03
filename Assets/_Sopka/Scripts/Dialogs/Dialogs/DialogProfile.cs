using bog.Serialization;

namespace Whaledevelop.Dialogs
{
    public class DialogProfile : ISerializableData
    {
        public DialogProfile(string dialogId)
        {
            DialogId = dialogId;
        }

        public string DialogId { get; set; }
        public string NodeId { get; set; }

        public override string ToString()
        {
            return $"DialogId:{DialogId}, NodeId:{NodeId}";
        }

        #region ISerializableData

        void ISerializableData.ToByteWriter(ByteWriter byteWriter)
        {
            byteWriter.WriteString(DialogId);
            byteWriter.WriteString(NodeId);
        }

        void ISerializableData.FromByteReader(ByteReader byteReader)
        {
            DialogId = byteReader.ReadString();
            NodeId = byteReader.ReadString();
        }

        #endregion
    }
}