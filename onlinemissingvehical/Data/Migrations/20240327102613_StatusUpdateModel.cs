using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace onlinemissingvehical.Data.Migrations
{
    /// <inheritdoc />
    public partial class StatusUpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StatusUpdates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MissingVehicleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusUpdates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusUpdates_MissingVehicles_MissingVehicleId",
                        column: x => x.MissingVehicleId,
                        principalTable: "MissingVehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatusUpdates_MissingVehicleId",
                table: "StatusUpdates",
                column: "MissingVehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatusUpdates");
        }
    }
}
