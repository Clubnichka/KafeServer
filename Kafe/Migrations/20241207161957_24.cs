using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kafe.Migrations
{
    /// <inheritdoc />
    public partial class _24 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Orders_OrderId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Promotions_PromotionId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_SpecialOffers_SpecialOfferId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialOffers_Orders_OrderId",
                table: "SpecialOffers");

            migrationBuilder.DropIndex(
                name: "IX_SpecialOffers_OrderId",
                table: "SpecialOffers");

            migrationBuilder.DropIndex(
                name: "IX_Products_OrderId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_PromotionId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SpecialOfferId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "SpecialOffers");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PromotionId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SpecialOfferId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "OrderProduct",
                columns: table => new
                {
                    OrdersId = table.Column<int>(type: "int", nullable: false),
                    ProductsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProduct", x => new { x.OrdersId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_OrderProduct_Orders_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderSpecialOffer",
                columns: table => new
                {
                    OrdersId = table.Column<int>(type: "int", nullable: false),
                    SpecialOffersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSpecialOffer", x => new { x.OrdersId, x.SpecialOffersId });
                    table.ForeignKey(
                        name: "FK_OrderSpecialOffer_Orders_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderSpecialOffer_SpecialOffers_SpecialOffersId",
                        column: x => x.SpecialOffersId,
                        principalTable: "SpecialOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPromotion",
                columns: table => new
                {
                    ProductsId = table.Column<int>(type: "int", nullable: false),
                    PromotionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPromotion", x => new { x.ProductsId, x.PromotionsId });
                    table.ForeignKey(
                        name: "FK_ProductPromotion_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductPromotion_Promotions_PromotionsId",
                        column: x => x.PromotionsId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSpecialOffer",
                columns: table => new
                {
                    ProductsId = table.Column<int>(type: "int", nullable: false),
                    SpecialOffersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSpecialOffer", x => new { x.ProductsId, x.SpecialOffersId });
                    table.ForeignKey(
                        name: "FK_ProductSpecialOffer_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSpecialOffer_SpecialOffers_SpecialOffersId",
                        column: x => x.SpecialOffersId,
                        principalTable: "SpecialOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_ProductsId",
                table: "OrderProduct",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderSpecialOffer_SpecialOffersId",
                table: "OrderSpecialOffer",
                column: "SpecialOffersId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPromotion_PromotionsId",
                table: "ProductPromotion",
                column: "PromotionsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecialOffer_SpecialOffersId",
                table: "ProductSpecialOffer",
                column: "SpecialOffersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProduct");

            migrationBuilder.DropTable(
                name: "OrderSpecialOffer");

            migrationBuilder.DropTable(
                name: "ProductPromotion");

            migrationBuilder.DropTable(
                name: "ProductSpecialOffer");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "SpecialOffers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PromotionId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpecialOfferId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpecialOffers_OrderId",
                table: "SpecialOffers",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrderId",
                table: "Products",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_PromotionId",
                table: "Products",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SpecialOfferId",
                table: "Products",
                column: "SpecialOfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Orders_OrderId",
                table: "Products",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Promotions_PromotionId",
                table: "Products",
                column: "PromotionId",
                principalTable: "Promotions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SpecialOffers_SpecialOfferId",
                table: "Products",
                column: "SpecialOfferId",
                principalTable: "SpecialOffers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialOffers_Orders_OrderId",
                table: "SpecialOffers",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
