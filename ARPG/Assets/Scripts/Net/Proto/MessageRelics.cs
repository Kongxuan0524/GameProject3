//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: MessageRelics.proto
// Note: requires additional types generated from: MessageID.proto
// Note: requires additional types generated from: MessageRetCode.proto
// Note: requires additional types generated from: MessageCommon.proto
namespace Protocol
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ReqChargeRelics")]
  public partial class ReqChargeRelics : global::ProtoBuf.IExtensible
  {
    public ReqChargeRelics() {}
    
    private int _RelicsID;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"RelicsID", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int RelicsID
    {
      get { return _RelicsID; }
      set { _RelicsID = value; }
    }
    private int _Index;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"Index", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int Index
    {
      get { return _Index; }
      set { _Index = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"AckChargeRelics")]
  public partial class AckChargeRelics : global::ProtoBuf.IExtensible
  {
    public AckChargeRelics() {}
    
    private int _RelicsID;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"RelicsID", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int RelicsID
    {
      get { return _RelicsID; }
      set { _RelicsID = value; }
    }
    private int _Index;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"Index", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int Index
    {
      get { return _Index; }
      set { _Index = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ReqUpgradeRelics")]
  public partial class ReqUpgradeRelics : global::ProtoBuf.IExtensible
  {
    public ReqUpgradeRelics() {}
    
    private int _RelicsID;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"RelicsID", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int RelicsID
    {
      get { return _RelicsID; }
      set { _RelicsID = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"AckUpgradeRelics")]
  public partial class AckUpgradeRelics : global::ProtoBuf.IExtensible
  {
    public AckUpgradeRelics() {}
    
    private int _RelicsID;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"RelicsID", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int RelicsID
    {
      get { return _RelicsID; }
      set { _RelicsID = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ReqBattleRelics")]
  public partial class ReqBattleRelics : global::ProtoBuf.IExtensible
  {
    public ReqBattleRelics() {}
    
    private int _RelicsID;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"RelicsID", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int RelicsID
    {
      get { return _RelicsID; }
      set { _RelicsID = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"AckBattleRelics")]
  public partial class AckBattleRelics : global::ProtoBuf.IExtensible
  {
    public AckBattleRelics() {}
    
    private int _RelicsID;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"RelicsID", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int RelicsID
    {
      get { return _RelicsID; }
      set { _RelicsID = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ReqUnloadRelics")]
  public partial class ReqUnloadRelics : global::ProtoBuf.IExtensible
  {
    public ReqUnloadRelics() {}
    
    private int _RelicsID;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"RelicsID", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int RelicsID
    {
      get { return _RelicsID; }
      set { _RelicsID = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"AckUnloadRelics")]
  public partial class AckUnloadRelics : global::ProtoBuf.IExtensible
  {
    public AckUnloadRelics() {}
    
    private int _RelicsID;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"RelicsID", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int RelicsID
    {
      get { return _RelicsID; }
      set { _RelicsID = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}