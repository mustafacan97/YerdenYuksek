using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace YerdenYuksek.Web.Framework.Migrations._05072023
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ActivityLogType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SystemKeyword = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Active = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogType", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Email = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstName = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gender = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", precision: 6, nullable: true),
                    EmailValidated = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PhoneNumberValidated = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RequireReLogin = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FailedLoginAttempts = table.Column<int>(type: "int", nullable: false),
                    CannotLoginUntilDateUtc = table.Column<DateTime>(type: "datetime(6)", precision: 6, nullable: true),
                    Active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastIpAddress = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime(6)", precision: 6, nullable: false),
                    LastLoginDateUtc = table.Column<DateTime>(type: "datetime(6)", precision: 6, nullable: true),
                    LastActivityDateUtc = table.Column<DateTime>(type: "datetime(6)", precision: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CustomerRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerRole", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EmailAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Email = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Host = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Port = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EnableSsl = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAccount", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LanguageCulture = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UniqueSeoCode = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FlagImageFileName = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rtl = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsDefaultLanguage = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Setting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ActivityLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ActivityLogTypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CustomerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EntityId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    IpAddress = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Comment = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityLog_ActivityLogType_ActivityLogTypeId",
                        column: x => x.ActivityLogTypeId,
                        principalTable: "ActivityLogType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityLog_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CustomerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FirstName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CountryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    City = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Address1 = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address2 = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ZipCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CustomerPassword",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordFormatId = table.Column<int>(type: "int", nullable: false),
                    PasswordSalt = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime(6)", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPassword", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_CustomerPassword_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CustomerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ShortMessage = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FullMessage = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IpAddress = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LogLevelId = table.Column<int>(type: "int", nullable: false),
                    EndpointUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime(6)", precision: 6, nullable: false),
                    LogLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Log_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CustomerRoleMapping",
                columns: table => new
                {
                    CustomerRoleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CustomerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerRoleMapping", x => new { x.CustomerRoleId, x.CustomerId });
                    table.ForeignKey(
                        name: "FK_CustomerRoleMapping_CustomerRole_CustomerRoleId",
                        column: x => x.CustomerRoleId,
                        principalTable: "CustomerRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerRoleMapping_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MessageTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BccEmailAddresses = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Subject = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Body = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailAccountId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageTemplate_EmailAccount_EmailAccountId",
                        column: x => x.EmailAccountId,
                        principalTable: "EmailAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "CustomerRole",
                columns: new[] { "Id", "Active", "CreatedOnUtc", "Deleted", "Name" },
                values: new object[,]
                {
                    { new Guid("1b48c281-e97f-4e8d-82fe-41ad1dde09b0"), true, new DateTime(2023, 7, 5, 15, 12, 32, 794, DateTimeKind.Utc).AddTicks(4907), false, "Vendors" },
                    { new Guid("2086daa6-323b-4bfe-b982-6eb485773718"), true, new DateTime(2023, 7, 5, 15, 12, 32, 794, DateTimeKind.Utc).AddTicks(4904), false, "Registered" },
                    { new Guid("4e772575-fb0f-4d10-b8fa-6dbfe0c6fe4f"), true, new DateTime(2023, 7, 5, 15, 12, 32, 794, DateTimeKind.Utc).AddTicks(4882), false, "Administrators" },
                    { new Guid("cfae18cb-3b8d-4ece-bdba-e2a05c920459"), true, new DateTime(2023, 7, 5, 15, 12, 32, 794, DateTimeKind.Utc).AddTicks(4906), false, "Guests" },
                    { new Guid("dff4bc34-01ec-46c8-a278-58273badef67"), true, new DateTime(2023, 7, 5, 15, 12, 32, 794, DateTimeKind.Utc).AddTicks(4902), false, "ForumModerators" }
                });

            migrationBuilder.InsertData(
                table: "EmailAccount",
                columns: new[] { "Id", "Active", "Deleted", "DisplayName", "Email", "EnableSsl", "Host", "Password", "Port", "Username" },
                values: new object[] { new Guid("fd5134ba-eb25-4c0d-81e1-b9639c1267c8"), true, false, "Store name", "test@mail.com", false, "smtp.mail.com", "123", 25, "123" });

            migrationBuilder.InsertData(
                table: "Language",
                columns: new[] { "Id", "Active", "Deleted", "DisplayOrder", "FlagImageFileName", "IsDefaultLanguage", "LanguageCulture", "Name", "Rtl", "UniqueSeoCode" },
                values: new object[] { new Guid("50604336-8bfc-4c64-b537-8336a07ac0c4"), true, false, 1, "tr.png", true, "tr-TR", "TR", false, "tr" });

            migrationBuilder.InsertData(
                table: "Setting",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { new Guid("0098e9e7-00ab-46c7-9297-3a63338608b0"), "customersettings.storelastvisitedpage", "False" },
                    { new Guid("02999389-bfb7-4ecf-b375-a12358c6a903"), "customersettings.suffixdeletedcustomers", "False" },
                    { new Guid("038377a5-5279-46bc-b1db-322892f35722"), "customersettings.streetaddressrequired", "False" },
                    { new Guid("0700f9ab-1fe6-4ce0-800e-b65c0e4f3512"), "customersettings.streetaddress2enabled", "False" },
                    { new Guid("096599da-6d21-4c17-a5eb-789075b34130"), "customersettings.phonenumbervalidationenabled", "False" },
                    { new Guid("0e6b67ef-2c8b-45df-9a18-4f1fba45b328"), "customersettings.showcustomersjoindate", "False" },
                    { new Guid("1850d28b-83d9-476a-b76c-dd17981915e0"), "customersettings.countryrequired", "False" },
                    { new Guid("1a079066-6d12-48d3-93d5-d750f8a84ba9"), "customersettings.passwordrequiredigit", "False" },
                    { new Guid("1dffbc77-da99-4b71-9be2-cb914b9f03b2"), "messagesettings.usepopupnotifications", "False" },
                    { new Guid("1f1c218e-a1dc-40f8-a2ca-6f85b71f20ea"), "securitysettings.useaesencryptionalgorithm", "True" },
                    { new Guid("21d28451-d551-499f-a56f-02a93ea61afc"), "customersettings.cityrequired", "False" },
                    { new Guid("22e54364-d9e3-4736-9297-e6e17bb54b68"), "customersettings.phoneenabled", "False" },
                    { new Guid("251950de-da31-4add-81db-fd598901263e"), "customersettings.zipcodeenabled", "False" },
                    { new Guid("38ba957b-2643-40be-b5d5-51c193e70729"), "customersettings.allowuserstochangeusernames", "False" },
                    { new Guid("3cb3c84e-0d50-46e1-861d-1bf496f901ce"), "securitysettings.allownonasciicharactersinheaders", "True" },
                    { new Guid("40c05aa6-0f7e-41c3-bd2a-0c57e9fb30b7"), "customersettings.defaultavatarenabled", "True" },
                    { new Guid("452f2c71-cf65-4f51-ad70-c20faf7caf51"), "customersettings.lastnameenabled", "True" },
                    { new Guid("476d8426-184d-4c72-8e27-4a0f6ff21e23"), "customersettings.streetaddress2required", "False" },
                    { new Guid("480a09d6-3bd7-4dea-9482-e793412b36d6"), "customersettings.deleteguesttaskolderthanminutes", "1440" },
                    { new Guid("491b1fb5-6a49-47e8-ba98-f38fe08be0c4"), "securitysettings.encryptionkey", "8831297055374565" },
                    { new Guid("4e4873ba-38eb-4e4e-a92d-cfb4c950bb10"), "customersettings.dateofbirthrequired", "False" },
                    { new Guid("56225459-57c9-47ee-87d5-9a424c14eae9"), "customersettings.failedpasswordlockoutminutes", "30" },
                    { new Guid("5892b8f4-d088-46f9-9524-30d412ad8cec"), "customersettings.avatarmaximumsizebytes", "20000" },
                    { new Guid("5bbaf01b-36b3-4bbd-8765-7dfcee3d5a52"), "customersettings.hidebackinstocksubscriptionstab", "False" },
                    { new Guid("5c4e7c54-2ed3-49ee-bcd0-0d7c2ec987e8"), "customersettings.requireregistrationfordownloadableproducts", "False" },
                    { new Guid("5f090644-3a66-45d0-8f35-c6fa95634595"), "customersettings.newslettertickedbydefault", "True" },
                    { new Guid("627963a9-4f64-4e6d-bc67-bf730733ef7d"), "customersettings.usernamevalidationuseregex", "False" },
                    { new Guid("62949af5-d26e-4380-b596-472c246d2211"), "customersettings.acceptprivacypolicyenabled", "False" },
                    { new Guid("666effbe-b66d-4f60-832a-96028c81fc40"), "messagesettings.usedefaultemailaccountforsendstoreowneremails", "False" },
                    { new Guid("71a8e97f-b0e6-4962-8267-0a292b497191"), "customersettings.onlinecustomerminutes", "20" },
                    { new Guid("722e316c-cde7-44dc-a3dc-b600195a36b3"), "customersettings.enteringemailtwice", "False" },
                    { new Guid("74523357-ae56-4d38-ab43-db1c675ca4fd"), "customersettings.usernamevalidationenabled", "False" },
                    { new Guid("7eb965d7-3214-434c-adfd-7dd6f700015d"), "customersettings.firstnameenabled", "True" },
                    { new Guid("7f2c0044-bdfc-4bc3-b672-2152dcb4cf3e"), "customersettings.hidedownloadableproductstab", "False" },
                    { new Guid("800dd625-ee96-436c-98d1-25fd92eba5e0"), "securitysettings.honeypotenabled", "False" },
                    { new Guid("82ee501f-4259-4614-81e5-7b2976428f4c"), "customersettings.passwordrequireuppercase", "False" },
                    { new Guid("83b0350a-1d82-4307-a725-a7c8f6b47c11"), "customersettings.notifynewcustomerregistration", "False" },
                    { new Guid("85049625-fa7d-42a1-962c-2235cd076461"), "customersettings.newsletterenabled", "True" },
                    { new Guid("85d53ddb-1023-4ad8-a98e-a198ef2252e0"), "customersettings.dateofbirthminimumage", "" },
                    { new Guid("880cd7b4-44cb-45f6-9111-2c00b938e565"), "customersettings.passwordrequirelowercase", "False" },
                    { new Guid("8d4b78ce-7eb2-4289-a9da-07e2ab5bd180"), "emailaccountsettings.defaultemailaccountid", "fd5134ba-eb25-4c0d-81e1-b9639c1267c8" },
                    { new Guid("9053c876-21b2-43a5-a0d2-7e031d037d2e"), "customersettings.failedpasswordallowedattempts", "0" },
                    { new Guid("96fabc77-5e81-4382-a5ee-070733c0315b"), "customersettings.phonenumbervalidationuseregex", "False" },
                    { new Guid("9df5fd0a-e1b0-4508-9b04-c494f58810c3"), "customersettings.cityenabled", "False" },
                    { new Guid("a2602499-86a7-41d0-96e8-0f500e72c013"), "customersettings.hashedpasswordformat", "SHA512" },
                    { new Guid("a5127d6d-f44f-475d-83ba-205f07624284"), "customersettings.countyenabled", "False" },
                    { new Guid("a587d395-cd2d-495e-817f-829d5466ff94"), "customersettings.defaultcountryid", "" },
                    { new Guid("a982ef27-106d-4ea4-b52d-6f793e367314"), "customersettings.streetaddressenabled", "False" },
                    { new Guid("ae9ca4d1-7a05-48b7-ae24-f3eafdc7deaa"), "customersettings.passwordrequirenonalphanumeric", "False" },
                    { new Guid("b05733b0-a352-4e3c-abfc-e33f96b75a4e"), "customersettings.phonenumbervalidationrule", "^[0-9]{1,14}?$" },
                    { new Guid("b16c669e-16f0-4271-8c58-d06c8201e4b3"), "customersettings.passwordmaxlength", "64" },
                    { new Guid("b2e38a07-e51d-4a83-a0c1-2e4758ac2c0f"), "customersettings.passwordlifetime", "90" },
                    { new Guid("bce875dd-15d9-4887-b74e-344b5e1d99f7"), "securitysettings.allowstoreownerexportimportcustomerswithhashedpassword", "True" },
                    { new Guid("be498312-4405-4b28-ae0d-2499b846a41d"), "customersettings.storeipaddresses", "True" },
                    { new Guid("bf435a8d-ddf7-4638-a331-6d6b493b39e6"), "customersettings.countryenabled", "False" },
                    { new Guid("c0547ebe-d616-41dd-bd75-9cbfeed1dc17"), "customersettings.zipcoderequired", "False" },
                    { new Guid("c2c5df08-baaa-4df7-a62c-5239f01f5e51"), "customersettings.usernamesenabled", "False" },
                    { new Guid("c8b76e76-ff7f-4947-8649-3ea140ef4708"), "customersettings.hidenewsletterblock", "False" },
                    { new Guid("ca1ed8d6-f51c-4d9b-8b75-abc5c6a0b683"), "customersettings.defaultpasswordformat", "Hashed" },
                    { new Guid("caff18f1-ea97-48d4-b0db-1e48979fd54c"), "customersettings.lastactivityminutes", "15" },
                    { new Guid("ce1b64cc-2776-4bae-a73d-de0eb7e9d81e"), "customersettings.downloadableproductsvalidateuser", "False" },
                    { new Guid("cef82896-2ad2-41de-8018-bb0ecb23f66b"), "customersettings.genderenabled", "True" },
                    { new Guid("d3c2d9ce-5718-479f-9c87-e2d3504239a2"), "securitysettings.honeypotinputname", "hpinput" },
                    { new Guid("d7655faf-d722-416c-bb83-ef70bc613249"), "customersettings.usernamevalidationrule", "" },
                    { new Guid("d78d9353-d061-42bd-8b05-7d26eb380cca"), "customersettings.lastnamerequired", "True" },
                    { new Guid("dadf2025-e538-480c-af3a-85626673f8ac"), "customersettings.passwordminlength", "6" },
                    { new Guid("db09c2c8-5dff-4490-a254-6a1b10657786"), "customersettings.allowcustomerstocheckgiftcardbalance", "False" },
                    { new Guid("dbb29e50-cf4e-4521-8bec-982d8785a9e5"), "customersettings.allowviewingprofiles", "False" },
                    { new Guid("df24c306-80ba-45e9-ba42-b724dc013b3d"), "customersettings.checkusernameavailabilityenabled", "False" },
                    { new Guid("e1ddf61e-d6eb-4418-8a9c-cb875e83d556"), "customersettings.phonerequired", "False" },
                    { new Guid("e550c700-0894-4e47-a83c-26fe307027f4"), "customersettings.unduplicatedpasswordsnumber", "4" },
                    { new Guid("e787434a-92aa-40f4-a8b2-bd6d7f8a57ea"), "customersettings.dateofbirthenabled", "False" },
                    { new Guid("e807cd7c-01f2-47f0-9813-bf5d821e4bcc"), "customersettings.firstnamerequired", "True" },
                    { new Guid("ebd7bcf8-e4ab-494f-b281-dc6529a625bc"), "customersettings.showcustomerslocation", "False" },
                    { new Guid("eef44b3e-0bb9-4c44-a73c-38b7dbf3acd1"), "customersettings.allowcustomerstouploadavatars", "False" },
                    { new Guid("f2e04d8a-e06e-4903-b254-ac7aa3bb8bde"), "customersettings.countyrequired", "False" },
                    { new Guid("fd922d63-fcfe-426e-8564-9e3f682f0246"), "customersettings.newsletterblockallowtounsubscribe", "False" },
                    { new Guid("ff73ebdd-9b8b-44c0-b3cf-e0fcaea0ddfa"), "customersettings.passwordrecoverylinkdaysvalid", "7" }
                });

            migrationBuilder.InsertData(
                table: "MessageTemplate",
                columns: new[] { "Id", "Active", "BccEmailAddresses", "Body", "Deleted", "EmailAccountId", "Name", "Subject" },
                values: new object[] { new Guid("e37f0e7f-e00b-4998-b171-7da59efbe8fe"), true, null, "We welcome you to <a href=\"%Store.URL%\"> %Store.Name%</a>.\r\n<br />\r\n<br />\r\nYou can now take part in the various services we have to offer you. Some of these services include:\r\n<br />\r\n<br />\r\nPermanent Cart - Any products added to your online cart remain there until you remove them, or check them out.\r\n<br />\r\nAddress Book - We can now deliver your products to another address other than yours! This is perfect to send birthday gifts direct to the birthday-person themselves.\r\n<br />\r\nOrder History - View your history of purchases that you have made with us.\r\n<br />\r\nProducts Reviews - Share your opinions on products with our other customers.\r\n<br />\r\n<br />\r\nFor help with any of our online services, please email the store-owner: <a href=\"mailto:%Store.Email%\">%Store.Email%</a>.\r\n<br />\r\n<br />\r\nNote: This email address was provided on our registration page. If you own the email and did not register on our site, please send an email to <a href=\"mailto:%Store.Email%\">%Store.Email%</a>.\r\n", false, new Guid("fd5134ba-eb25-4c0d-81e1-b9639c1267c8"), "Customer.WelcomeMessage", "Welcome to %Store.Name%" });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLog_ActivityLogTypeId",
                table: "ActivityLog",
                column: "ActivityLogTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLog_CustomerId",
                table: "ActivityLog",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_CustomerId",
                table: "Address",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRoleMapping_CustomerId",
                table: "CustomerRoleMapping",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_CustomerId",
                table: "Log",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageTemplate_EmailAccountId",
                table: "MessageTemplate",
                column: "EmailAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLog");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "CustomerPassword");

            migrationBuilder.DropTable(
                name: "CustomerRoleMapping");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "MessageTemplate");

            migrationBuilder.DropTable(
                name: "Setting");

            migrationBuilder.DropTable(
                name: "ActivityLogType");

            migrationBuilder.DropTable(
                name: "CustomerRole");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "EmailAccount");
        }
    }
}
