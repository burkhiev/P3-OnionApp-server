using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace AppInfrastructure.Migrations
{
    public partial class addinitdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Accounts",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountType", "CreatedAt", "Email", "Password" },
                values: new object[,]
                {
                    { new Guid("2067e7d9-6779-42e7-9620-806d6ea8323f"), "", NodaTime.Instant.FromUnixTimeTicks(16413630421975509L), "Jamar_Cole37@yahoo.com", "ZNbZ3dgqhO" },
                    { new Guid("28c2c279-23c4-4694-b409-dbfe0d6074d4"), "", NodaTime.Instant.FromUnixTimeTicks(16565859916859428L), "Ciara.Gleichner45@hotmail.com", "oXGP6SR4Mo" },
                    { new Guid("4ebe9237-d38e-4556-a7e8-03cab9e7f304"), "", NodaTime.Instant.FromUnixTimeTicks(16354202442014359L), "Monica.Ondricka33@hotmail.com", "oKdwtnDIwK" },
                    { new Guid("554dc1b5-aae0-47d1-8b0c-988d100975c6"), "", NodaTime.Instant.FromUnixTimeTicks(16393300679916038L), "Muhammad.White@yahoo.com", "BQzcsUYmLT" },
                    { new Guid("7466525d-d01e-4cdb-b134-11c448747d28"), "", NodaTime.Instant.FromUnixTimeTicks(16359337364693553L), "Jovany.Auer77@hotmail.com", "BuwwACbGSz" },
                    { new Guid("bc5e0ff3-b7d6-4ef1-9526-accfda1ff051"), "", NodaTime.Instant.FromUnixTimeTicks(16371746448875471L), "Mossie59@gmail.com", "xEZNKolh2e" },
                    { new Guid("c016bc81-6c31-4433-bbfc-c6b2428f8e02"), "", NodaTime.Instant.FromUnixTimeTicks(16456332113416938L), "Mia_Ebert@yahoo.com", "1muuVO_0uS" },
                    { new Guid("ce3d6295-36ae-4ff5-8056-63a0894dea63"), "", NodaTime.Instant.FromUnixTimeTicks(16436878366004708L), "Jazlyn67@gmail.com", "Pvo7bEQDqI" },
                    { new Guid("d44de473-1b91-48e2-b55a-49bbf5a52f64"), "", NodaTime.Instant.FromUnixTimeTicks(16396551442101077L), "Deangelo.Hoeger@hotmail.com", "cYCk_fMYy9" },
                    { new Guid("f7ceda94-fde7-44e5-acdc-9dcc3b2907c5"), "", NodaTime.Instant.FromUnixTimeTicks(16515092102283558L), "Mona41@yahoo.com", "TXXMYmEIZR" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccountId", "DateOfBirth", "FirstName", "LastName" },
                values: new object[,]
                {
                    { new Guid("204f6065-535a-4664-b4ae-c4e03651ff80"), new Guid("2067e7d9-6779-42e7-9620-806d6ea8323f"), NodaTime.Instant.FromUnixTimeTicks(16336458467228436L), "Xavier", "Emard" },
                    { new Guid("3568c9f7-2bc6-440c-9c61-480eb4977035"), new Guid("bc5e0ff3-b7d6-4ef1-9526-accfda1ff051"), NodaTime.Instant.FromUnixTimeTicks(16371678946131415L), "Olen", "Goodwin" },
                    { new Guid("3b29a9d6-f5b6-41b0-bf44-10a9008bc67a"), new Guid("c016bc81-6c31-4433-bbfc-c6b2428f8e02"), NodaTime.Instant.FromUnixTimeTicks(16277325494770129L), "Edwardo", "Nikolaus" },
                    { new Guid("5651b272-1b9c-498c-8b71-2bfcecffb9e9"), new Guid("554dc1b5-aae0-47d1-8b0c-988d100975c6"), NodaTime.Instant.FromUnixTimeTicks(16546588409145686L), "Lou", "Bosco" },
                    { new Guid("64deb4a6-5688-4921-b67f-2c04de364f69"), new Guid("28c2c279-23c4-4694-b409-dbfe0d6074d4"), NodaTime.Instant.FromUnixTimeTicks(16491459564604299L), "Henderson", "Hartmann" },
                    { new Guid("6655a7c2-f784-43f0-9fd8-1d4e94f2c8f4"), new Guid("d44de473-1b91-48e2-b55a-49bbf5a52f64"), NodaTime.Instant.FromUnixTimeTicks(16489505447636581L), "Della", "Armstrong" },
                    { new Guid("82a70217-126b-4f47-8923-7a8709758629"), new Guid("f7ceda94-fde7-44e5-acdc-9dcc3b2907c5"), NodaTime.Instant.FromUnixTimeTicks(16496788319838336L), "Ervin", "D'Amore" },
                    { new Guid("c1db7065-f5d3-401a-b12e-dc39fb23c232"), new Guid("7466525d-d01e-4cdb-b134-11c448747d28"), NodaTime.Instant.FromUnixTimeTicks(16372585200939143L), "Elody", "Kirlin" },
                    { new Guid("d4be7e8f-ade7-463a-af91-7d311f1a787a"), new Guid("4ebe9237-d38e-4556-a7e8-03cab9e7f304"), NodaTime.Instant.FromUnixTimeTicks(16524434299847401L), "Asha", "Donnelly" },
                    { new Guid("fab9814d-be56-4f61-8d38-925b1ccaa43d"), new Guid("ce3d6295-36ae-4ff5-8056-63a0894dea63"), NodaTime.Instant.FromUnixTimeTicks(16470791903117221L), "Mikel", "Haley" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("204f6065-535a-4664-b4ae-c4e03651ff80"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3568c9f7-2bc6-440c-9c61-480eb4977035"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3b29a9d6-f5b6-41b0-bf44-10a9008bc67a"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("5651b272-1b9c-498c-8b71-2bfcecffb9e9"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("64deb4a6-5688-4921-b67f-2c04de364f69"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("6655a7c2-f784-43f0-9fd8-1d4e94f2c8f4"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("82a70217-126b-4f47-8923-7a8709758629"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c1db7065-f5d3-401a-b12e-dc39fb23c232"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d4be7e8f-ade7-463a-af91-7d311f1a787a"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("fab9814d-be56-4f61-8d38-925b1ccaa43d"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("2067e7d9-6779-42e7-9620-806d6ea8323f"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("28c2c279-23c4-4694-b409-dbfe0d6074d4"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("4ebe9237-d38e-4556-a7e8-03cab9e7f304"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("554dc1b5-aae0-47d1-8b0c-988d100975c6"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("7466525d-d01e-4cdb-b134-11c448747d28"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("bc5e0ff3-b7d6-4ef1-9526-accfda1ff051"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("c016bc81-6c31-4433-bbfc-c6b2428f8e02"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("ce3d6295-36ae-4ff5-8056-63a0894dea63"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("d44de473-1b91-48e2-b55a-49bbf5a52f64"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("f7ceda94-fde7-44e5-acdc-9dcc3b2907c5"));

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Accounts",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);
        }
    }
}
