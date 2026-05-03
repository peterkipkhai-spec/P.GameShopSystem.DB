# Game Shop System

Game Shop System is a clean-architecture sample for a game store platform with separated Domain, DB, API, and Web projects.

## User Stories

### 1) Authentication
- As a customer, I want to register with phone and password so I can create an account.
- As a customer, I want to login with phone and password so I can access shopping features.
- As a system admin, I want duplicate phone registration blocked so user accounts stay unique.

### 2) Category Management (Admin)
- As an admin, I want to create categories so games are organized.
- As an admin, I want to edit category name/description so data stays up-to-date.
- As an admin, I want to remove unused categories so catalog is clean.

### 3) Game Management (Admin)
- As an admin, I want to add games with category, price, stock, and condition so products can be sold.
- As an admin, I want to update game details so catalog data is accurate.
- As an admin, I want to disable unavailable games so users do not buy invalid items.

### 4) Game Browsing (Customer)
- As a customer, I want to browse games by category so I can find products faster.
- As a customer, I want to view game details so I can decide before buying.

### 5) Cart System
- As a customer, I want to add games to cart so I can checkout multiple items together.
- As a customer, I want to update quantities in cart so I can control order totals.
- As a customer, I want to remove cart items so I can correct my order.

### 6) Order System
- As a customer, I want to place an order from cart so I can purchase games.
- As a system, I want to calculate total amount from order items so billing is correct.

### 7) Order History
- As a customer, I want to see my previous orders so I can track purchases.
- As a customer, I want to view each order's status so I know processing progress.

### 8) Admin Order Management
- As an admin, I want to see all orders so I can process fulfillment.
- As an admin, I want to update order status (Pending/Processing/Completed/Cancelled) so workflow is controlled.

## Solution Structure
- `P.GameShopSystem.Domain` → domain constants/entities.
- `P.GameShopSystem.DB` → EF Core database models and DbContext.
- `P.GameShopSystem.API` → API endpoints and business logic.
- `P.GameShopSystem.Web` → MVC web client consuming API.
