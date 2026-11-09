using LegacyVault.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LegacyVault.Data.Migrations;

[DbContext(typeof(ApplicationDbContext))]
[Migration("20261108120000_AddEncryptedAssetCredentials")]
public partial class AddEncryptedAssetCredentials : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "EncryptedCredentials",
            table: "DigitalAssets",
            type: "TEXT",
            maxLength: 3000,
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "EncryptedCredentials",
            table: "DigitalAssets");
    }
}