using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class GetPersons_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Persons_CountryId",
                table: "Persons",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Countries_CountryId",
                table: "Persons",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId");

            string sp_GetAllPersons = @"
                CREATE OR REPLACE FUNCTION get_all_persons()
                RETURNS TABLE (
                personid uuid,
                personname text,
                dateofbirth timestamp,
                email text,
                countryid uuid,
                gender text,
                address text,
                receivenewsletter boolean
                )
                LANGUAGE plpgsql
                AS $$
                BEGIN
                RETURN QUERY
                SELECT 
                ""PersonId"",
                 ""PersonName"",
                ""DateOfBirth"",
                ""Email"",
                ""CountryId"",
                ""Gender"",
                ""Address"",
                ""ReceiveNewsLetter""
                FROM ""Persons"";
                END;
                $$;
                   ";
            migrationBuilder.Sql(sp_GetAllPersons);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Countries_CountryId",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Persons_CountryId",
                table: "Persons");
        }
    }
}
