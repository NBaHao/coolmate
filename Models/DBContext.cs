using System;
using System.Collections.Generic;
using CoolMate.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoolMate.Models;

public partial class DBContext : IdentityDbContext<SiteUser>
{
    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<OrderLine> OrderLines { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductItem> ProductItems { get; set; }

    public virtual DbSet<ProductItemImage> ProductItemImages { get; set; }

    public virtual DbSet<ShopOrder> ShopOrders { get; set; }

    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    public virtual DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

    public virtual DbSet<SiteUser> SiteUsers { get; set; }

    public virtual DbSet<UserAddress> UserAddresses { get; set; }

    public virtual DbSet<UserReview> UserReviews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("Server=127.0.0.1;User ID=root;Password=anhyeuem;Port=3306;Database=coolmate");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityUserLogin<String>>().HasNoKey();
        modelBuilder.Entity<IdentityUserRole<string>>(b =>
        {
            b.HasKey(i => new { i.UserId, i.RoleId });
        });

        modelBuilder.Entity<IdentityUserToken<String>>().HasNoKey();
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("address");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddressLine)
                .HasMaxLength(500)
                .HasColumnName("address_line");
            entity.Property(e => e.Commune)
                .HasMaxLength(200)
                .HasColumnName("commune");
            entity.Property(e => e.District)
                .HasMaxLength(200)
                .HasColumnName("district");
            entity.Property(e => e.Province)
                .HasMaxLength(200)
                .HasColumnName("province");
        });

        modelBuilder.Entity<OrderLine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("order_line");

            entity.HasIndex(e => e.OrderId, "fk_orderline_order");

            entity.HasIndex(e => e.ProductItemId, "fk_orderline_proditem");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ProductItemId).HasColumnName("product_item_id");
            entity.Property(e => e.Qty).HasColumnName("qty");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderLines)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("fk_orderline_order");

            entity.HasOne(d => d.ProductItem).WithMany(p => p.OrderLines)
                .HasForeignKey(d => d.ProductItemId)
                .HasConstraintName("fk_orderline_proditem");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("product");

            entity.HasIndex(e => e.CategoryId, "fk_category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.PriceInt).HasColumnName("price_int");
            entity.Property(e => e.PriceStr)
                .HasMaxLength(10)
                .HasColumnName("price_str");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("fk_category");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("product_category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Slug).HasMaxLength(200).HasColumnName("slug");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(200)
                .HasColumnName("category_name");
            entity.Property(e => e.ParentCategoryId).HasColumnName("parent_category_id");
        });

        modelBuilder.Entity<ProductItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("product_item");

            entity.HasIndex(e => e.ProductId, "fk_proditem_product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .HasColumnName("color");
            entity.Property(e => e.ColorImage)
                .HasMaxLength(1000)
                .HasColumnName("color_image");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.QtyInStock).HasColumnName("qty_in_stock");
            entity.Property(e => e.Size)
                .HasMaxLength(10)
                .HasColumnName("size");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("fk_proditem_product");
        });

        modelBuilder.Entity<ProductItemImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("product_item_image");

            entity.HasIndex(e => e.ProductItemId, "fk_product_item_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProductItemId).HasColumnName("product_item_id");
            entity.Property(e => e.Url)
                .HasMaxLength(200)
                .HasColumnName("url");

            entity.HasOne(d => d.ProductItem).WithMany(p => p.ProductItemImages)
                .HasForeignKey(d => d.ProductItemId)
                .HasConstraintName("fk_product_item_id");
        });

        modelBuilder.Entity<ShopOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("shop_order");

            entity.HasIndex(e => e.PaymentMethod, "fk_shoporder_paymethod");

            entity.HasIndex(e => e.ShippingMethod, "fk_shoporder_shipmethod");

            entity.HasIndex(e => e.OrderStatus, "fk_shoporder_status");

            entity.HasIndex(e => e.UserId, "fk_shoporder_user");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasColumnName("order_date");
            entity.Property(e => e.OrderStatus).HasColumnName("order_status");
            entity.Property(e => e.OrderTotal).HasColumnName("order_total");
            entity.Property(e => e.PaymentMethod).HasColumnName("payment_method");
            entity.Property(e => e.ShippingAddress).HasMaxLength(200).HasColumnName("shipping_address");
            entity.Property(e => e.Name).HasMaxLength(200).HasColumnName("name");
            entity.Property(e => e.Phone).HasMaxLength(10).HasColumnName("phone");
            entity.Property(e => e.Email).HasMaxLength(200).HasColumnName("email");
            entity.Property(e => e.ShippingMethod).HasColumnName("shipping_method");
            entity.Property(e => e.UserId).HasColumnName("user_id");


            entity.HasOne(d => d.User).WithMany(p => p.ShopOrders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_shoporder_user");
        });

        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("shopping_cart");

            entity.HasIndex(e => e.UserId, "fk_shopcart_user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.ShoppingCarts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_shopcart_user");
        });

        modelBuilder.Entity<ShoppingCartItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("shopping_cart_item");

            entity.HasIndex(e => e.ProductItemId, "fk_shopcartitem_proditem");

            entity.HasIndex(e => e.CartId, "fk_shopcartitem_shopcart");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.ProductItemId).HasColumnName("product_item_id");
            entity.Property(e => e.Qty).HasColumnName("qty");

            entity.HasOne(d => d.Cart).WithMany(p => p.ShoppingCartItems)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("fk_shopcartitem_shopcart");

            entity.HasOne(d => d.ProductItem).WithMany(p => p.ShoppingCartItems)
                .HasForeignKey(d => d.ProductItemId)
                .HasConstraintName("fk_shopcartitem_proditem");
        });

        modelBuilder.Entity<SiteUser>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("site_user");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
            entity.Property(e => e.Gender).HasMaxLength(8).HasColumnName("gender");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.Weight).HasColumnName("weight");
            entity.Property(e => e.Birthday).HasMaxLength(10).HasColumnName("birthday");
        });

        modelBuilder.Entity<UserAddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("user_address");

            entity.HasIndex(e => e.AddressId, "fk_useradd_address");

            entity.HasIndex(e => e.UserId, "fk_useradd_user");

            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.IsDefault).HasColumnName("is_default");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");

            entity.HasOne(d => d.Address).WithMany()
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("fk_useradd_address");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_useradd_user");
        });


        modelBuilder.Entity<UserReview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user_review");

            entity.HasIndex(e => e.OrderedProductId, "fk_review_product");

            entity.HasIndex(e => e.UserId, "fk_review_user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Comment)
                .HasMaxLength(2000)
                .HasColumnName("comment");
            entity.Property(e => e.OrderedProductId).HasColumnName("ordered_product_id");
            entity.Property(e => e.RatingValue).HasColumnName("rating_value");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(entity => entity.CreatedDate).HasColumnName("created_date");

            entity.HasOne(d => d.OrderedProduct).WithMany(p => p.UserReviews)
                .HasForeignKey(d => d.OrderedProductId)
                .HasConstraintName("fk_review_product");

            entity.HasOne(d => d.User).WithMany(p => p.UserReviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_review_user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
