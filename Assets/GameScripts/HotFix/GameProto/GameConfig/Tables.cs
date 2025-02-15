//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;


namespace GameConfig
{
   
public partial class Tables
{
    public item.TbItem TbItem {get; }
    public Battle.TbSkill TbSkill {get; }
    public Battle.TbBuff TbBuff {get; }
    public Battle.TbBuffAttr TbBuffAttr {get; }

    public Tables(System.Func<string, ByteBuf> idxLoader,System.Func<string, ByteBuf> dataLoader)
    {
        var tables = new System.Collections.Generic.Dictionary<string, object>();
        TbItem = new item.TbItem(idxLoader("item_tbitem"),"item_tbitem",dataLoader); 
        tables.Add("item.TbItem", TbItem);
        TbSkill = new Battle.TbSkill(idxLoader("battle_tbskill"),"battle_tbskill",dataLoader); 
        tables.Add("Battle.TbSkill", TbSkill);
        TbBuff = new Battle.TbBuff(idxLoader("battle_tbbuff"),"battle_tbbuff",dataLoader); 
        tables.Add("Battle.TbBuff", TbBuff);
        TbBuffAttr = new Battle.TbBuffAttr(idxLoader("battle_tbbuffattr"),"battle_tbbuffattr",dataLoader); 
        tables.Add("Battle.TbBuffAttr", TbBuffAttr);

        PostInit();
        TbItem.CacheTables(tables); 
        TbSkill.CacheTables(tables); 
        TbBuff.CacheTables(tables); 
        TbBuffAttr.CacheTables(tables); 
    }
    
    partial void PostInit();
}

}