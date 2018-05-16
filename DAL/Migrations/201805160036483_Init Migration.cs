namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EncodedTexts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InitialTextId = c.Int(nullable: false),
                        Value = c.String(nullable: false),
                        Keyword = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Texts", t => t.InitialTextId, cascadeDelete: true)
                .Index(t => t.InitialTextId);
            
            CreateTable(
                "dbo.Texts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.KasiskiResultItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KasiskiResultId = c.Int(nullable: false),
                        Size = c.Int(nullable: false),
                        Probability = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.KasiskiResults", t => t.KasiskiResultId, cascadeDelete: true)
                .Index(t => t.KasiskiResultId);
            
            CreateTable(
                "dbo.KasiskiResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EncodedTextId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EncodedTexts", t => t.EncodedTextId, cascadeDelete: true)
                .Index(t => t.EncodedTextId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KasiskiResultItems", "KasiskiResultId", "dbo.KasiskiResults");
            DropForeignKey("dbo.KasiskiResults", "EncodedTextId", "dbo.EncodedTexts");
            DropForeignKey("dbo.EncodedTexts", "InitialTextId", "dbo.Texts");
            DropIndex("dbo.KasiskiResults", new[] { "EncodedTextId" });
            DropIndex("dbo.KasiskiResultItems", new[] { "KasiskiResultId" });
            DropIndex("dbo.EncodedTexts", new[] { "InitialTextId" });
            DropTable("dbo.KasiskiResults");
            DropTable("dbo.KasiskiResultItems");
            DropTable("dbo.Texts");
            DropTable("dbo.EncodedTexts");
        }
    }
}
