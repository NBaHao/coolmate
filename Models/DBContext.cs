using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Entities;

public partial class DBContext : DbContext
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

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<PaymentType> PaymentTypes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductItem> ProductItems { get; set; }

    public virtual DbSet<ShippingFee> ShippingFees { get; set; }

    public virtual DbSet<ShippingMethod> ShippingMethods { get; set; }

    public virtual DbSet<ShopOrder> ShopOrders { get; set; }

    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    public virtual DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

    public virtual DbSet<SiteUser> SiteUsers { get; set; }

    public virtual DbSet<UserAddress> UserAddresses { get; set; }

    public virtual DbSet<UserPaymentMethod> UserPaymentMethods { get; set; }

    public virtual DbSet<UserReview> UserReviews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){ }
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=anhyeuem;database=ecommerce2");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("order_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .HasColumnName("status");
        });

        modelBuilder.Entity<PaymentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("payment_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Value)
                .HasMaxLength(100)
                .HasColumnName("value");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("product");

            entity.HasIndex(e => e.CategoryId, "fk_category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Description)
                .HasMaxLength(4000)
                .HasColumnName("description");
            entity.Property(e => e.Hover)
                .HasMaxLength(200)
                .HasColumnName("hover");
            entity.Property(e => e.Img)
                .HasMaxLength(200)
                .HasColumnName("img");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
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
            entity.Property(e => e.CategoryName)
                .HasMaxLength(200)
                .HasColumnName("category_name");
            entity.Property(e => e.ParentCategoryId).HasColumnName("parent_category_id");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("product_image");

            entity.HasIndex(e => e.ProductItemId, "fk_product_item_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProductItemId).HasColumnName("product_item_id");
            entity.Property(e => e.Url)
                .HasMaxLength(200)
                .HasColumnName("url");

            entity.HasOne(d => d.ProductItem).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductItemId)
                .HasConstraintName("fk_product_item_id");
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

        modelBuilder.Entity<ShippingFee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("shipping_fee");

            entity.HasIndex(e => e.ShippingAddress, "fk_shippingfee_shipaddress");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ShippingAddress).HasColumnName("shipping_address");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.ShippingAddressNavigation).WithMany(p => p.ShippingFees)
                .HasForeignKey(d => d.ShippingAddress)
                .HasConstraintName("fk_shippingfee_shipaddress");
        });

        modelBuilder.Entity<ShippingMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("shipping_method");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ShopOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("shop_order");

            entity.HasIndex(e => e.PaymentMethodId, "fk_shoporder_paymethod");

            entity.HasIndex(e => e.ShippingAddress, "fk_shoporder_shipaddress");

            entity.HasIndex(e => e.ShippingMethod, "fk_shoporder_shipmethod");

            entity.HasIndex(e => e.OrderStatus, "fk_shoporder_status");

            entity.HasIndex(e => e.UserId, "fk_shoporder_user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasColumnName("order_date");
            entity.Property(e => e.OrderStatus).HasColumnName("order_status");
            entity.Property(e => e.OrderTotal).HasColumnName("order_total");
            entity.Property(e => e.PaymentMethodId).HasColumnName("payment_method_id");
            entity.Property(e => e.ShippingAddress).HasColumnName("shipping_address");
            entity.Property(e => e.ShippingMethod).HasColumnName("shipping_method");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.OrderStatusNavigation).WithMany(p => p.ShopOrders)
                .HasForeignKey(d => d.OrderStatus)
                .HasConstraintName("fk_shoporder_status");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.ShopOrders)
                .HasForeignKey(d => d.PaymentMethodId)
                .HasConstraintName("fk_shoporder_paymethod");

            entity.HasOne(d => d.ShippingAddressNavigation).WithMany(p => p.ShopOrders)
                .HasForeignKey(d => d.ShippingAddress)
                .HasConstraintName("fk_shoporder_shipaddress");

            entity.HasOne(d => d.ShippingMethodNavigation).WithMany(p => p.ShopOrders)
                .HasForeignKey(d => d.ShippingMethod)
                .HasConstraintName("fk_shoporder_shipmethod");

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
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("site_user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(350)
                .HasColumnName("email_address");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(500)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
        });

        modelBuilder.Entity<UserAddress>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("user_address");

            entity.HasIndex(e => e.AddressId, "fk_useradd_address");

            entity.HasIndex(e => e.UserId, "fk_useradd_user");

            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.IsDefault).HasColumnName("is_default");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Address).WithMany()
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("fk_useradd_address");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_useradd_user");
        });

        modelBuilder.Entity<UserPaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user_payment_method");

            entity.HasIndex(e => e.PaymentTypeId, "fk_userpm_paytype");

            entity.HasIndex(e => e.UserId, "fk_userpm_user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .HasColumnName("account_number");
            entity.Property(e => e.ExpiryDate)
                .HasColumnType("date")
                .HasColumnName("expiry_date");
            entity.Property(e => e.IsDefault).HasColumnName("is_default");
            entity.Property(e => e.PaymentTypeId).HasColumnName("payment_type_id");
            entity.Property(e => e.Provider)
                .HasMaxLength(100)
                .HasColumnName("provider");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.PaymentType).WithMany(p => p.UserPaymentMethods)
                .HasForeignKey(d => d.PaymentTypeId)
                .HasConstraintName("fk_userpm_paytype");

            entity.HasOne(d => d.User).WithMany(p => p.UserPaymentMethods)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_userpm_user");
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
