using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace todoapp.server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Roll = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Account__3214EC0703D6E2F6", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tags__3214EC0782B50CF2", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "List",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__List__3214EC0731A512E6", x => x.Id);
                    table.ForeignKey(
                        name: "FK__List__AccountID__45F365D3",
                        column: x => x.AccountID,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StickyNote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("StickyNote_pk", x => x.Id);
                    table.ForeignKey(
                        name: "FK__StickyNot__Accou__46E78A0C",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    ListId = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Task__3214EC07E554C5CF", x => x.Id);
                    table.ForeignKey(
                        name: "Task_List_Id_fk",
                        column: x => x.ListId,
                        principalTable: "List",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubTask",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Id", x => x.Id);
                    table.ForeignKey(
                        name: "SubTask_Task_Id_fk",
                        column: x => x.TaskId,
                        principalTable: "Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagsTask",
                columns: table => new
                {
                    TagsId = table.Column<int>(type: "int", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: true, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TagsTask__D3F7FF67372D1309", x => new { x.TagsId, x.TaskId });
                    table.ForeignKey(
                        name: "FK__TagsTask__TagsId__440B1D61",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__TagsTask__TaskId__44FF419A",
                        column: x => x.TaskId,
                        principalTable: "Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_List_AccountID",
                table: "List",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_StickyNote_AccountId",
                table: "StickyNote",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTask_TaskId",
                table: "SubTask",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TagsTask_TaskId",
                table: "TagsTask",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_ListId",
                table: "Task",
                column: "ListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StickyNote");

            migrationBuilder.DropTable(
                name: "SubTask");

            migrationBuilder.DropTable(
                name: "TagsTask");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "List");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
