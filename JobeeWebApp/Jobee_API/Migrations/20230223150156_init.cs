using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jobee_API.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbTypeAccount",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbTypeAccount", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tbAccount",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IDTypeAccount = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Passwork = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbAccount", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tbAccount_tbTypeAccount",
                        column: x => x.IDTypeAccount,
                        principalTable: "tbTypeAccount",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "tbProfile",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IDAccount = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<bool>(type: "bit", nullable: false),
                    DoB = table.Column<DateTime>(type: "datetime", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SocialNetwork = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DetailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbProfile", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tbProfile_tbAccount",
                        column: x => x.IDAccount,
                        principalTable: "tbAccount",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "tbAdmin",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IDProfile = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbAdmin", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tbAdmin_tbProfile",
                        column: x => x.IDProfile,
                        principalTable: "tbProfile",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "tbEmployee",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IDProfile = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbEmployee", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tbEmployee_tbProfile",
                        column: x => x.IDProfile,
                        principalTable: "tbProfile",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "tbCV",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IDEmployee = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ApplyPosition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentJob = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DesirySalary = table.Column<decimal>(type: "money", nullable: false),
                    Degree = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkExperience = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DesiredWorkLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkingForm = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarrerObjiect = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoftSkill = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbCV", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tbCV_tbEmployee",
                        column: x => x.IDEmployee,
                        principalTable: "tbEmployee",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IDCV = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Activity_tbCV",
                        column: x => x.IDCV,
                        principalTable: "tbCV",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Award",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IDCV = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Award", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Award_tbCV",
                        column: x => x.IDCV,
                        principalTable: "tbCV",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Certificate",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IDCV = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificate", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Certificate_tbCV",
                        column: x => x.IDCV,
                        principalTable: "tbCV",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Education",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IDCV = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Major = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: false),
                    GPA = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Education", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Education_tbCV",
                        column: x => x.IDCV,
                        principalTable: "tbCV",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IDCV = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeamSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Technology = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Project_tbCV",
                        column: x => x.IDCV,
                        principalTable: "tbCV",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_IDCV",
                table: "Activity",
                column: "IDCV");

            migrationBuilder.CreateIndex(
                name: "IX_Award_IDCV",
                table: "Award",
                column: "IDCV");

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_IDCV",
                table: "Certificate",
                column: "IDCV");

            migrationBuilder.CreateIndex(
                name: "IX_Education_IDCV",
                table: "Education",
                column: "IDCV");

            migrationBuilder.CreateIndex(
                name: "IX_Project_IDCV",
                table: "Project",
                column: "IDCV");

            migrationBuilder.CreateIndex(
                name: "IX_tbAccount_IDTypeAccount",
                table: "tbAccount",
                column: "IDTypeAccount");

            migrationBuilder.CreateIndex(
                name: "IX_tbAdmin_IDProfile",
                table: "tbAdmin",
                column: "IDProfile");

            migrationBuilder.CreateIndex(
                name: "IX_tbCV_IDEmployee",
                table: "tbCV",
                column: "IDEmployee");

            migrationBuilder.CreateIndex(
                name: "IX_tbEmployee_IDProfile",
                table: "tbEmployee",
                column: "IDProfile");

            migrationBuilder.CreateIndex(
                name: "IX_tbProfile_IDAccount",
                table: "tbProfile",
                column: "IDAccount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "Award");

            migrationBuilder.DropTable(
                name: "Certificate");

            migrationBuilder.DropTable(
                name: "Education");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "tbAdmin");

            migrationBuilder.DropTable(
                name: "tbCV");

            migrationBuilder.DropTable(
                name: "tbEmployee");

            migrationBuilder.DropTable(
                name: "tbProfile");

            migrationBuilder.DropTable(
                name: "tbAccount");

            migrationBuilder.DropTable(
                name: "tbTypeAccount");
        }
    }
}
