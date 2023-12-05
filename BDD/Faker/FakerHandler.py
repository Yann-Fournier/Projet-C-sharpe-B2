import random
import sqlite3
from faker import Faker

from hash import hash_passwd

con = sqlite3.connect("../identifier.sqlite")
global id_master


def create_user(i, login_info, address, photo, commands, cart, invoices, prefer_payment, rating, data=None):
    if data is None:
        data = [
            i, Faker().name(), login_info, address, photo,
            commands, cart, invoices, prefer_payment, rating
        ]
    con.execute(
        "INSERT INTO User (Id, Name, Login_info, Address, Photo, Commands, Cart, Invoices, Prefer_payment, "
        "Rating) VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", data)
    con.commit()


def create_product(i, photo, category, seller, rating, data=None):
    if data is None:
        data = [
            i, Faker().name(), Faker().random_int(), Faker().text(), photo,
            category, seller, rating
        ]
    print(data)
    con.execute(
        "INSERT INTO Items (Id, Name, Price, Description, Photo, Category, Seller, Rating)VALUES(?, ?, ?, ?, ?, ?, ?, "
        "?)", data)
    con.commit()


def create_photo(i, data=None):
    if data is None:
        data = [
            i, Faker().image_url()
        ]
    print(data)
    con.execute(
        "INSERT INTO Photo (Id, Link)VALUES(?, ?)", data)
    con.commit()


def create_category(i, data=None):
    if data is None:
        data = [
            i, Faker().job()
        ]
    print(data)
    con.execute(
        "INSERT INTO Category (Id, Name)VALUES(?, ?)", data)
    con.commit()


def create_address(i, data=None):
    if data is None:
        data = [
            i, Faker().street_address(), Faker().city(), Faker().random_int(), Faker().state(),
            Faker().country()
        ]
    print(data)
    con.execute(
        "INSERT INTO Address (Id, Street, City, CP, State, Country)VALUES(?, ?, ?, ?, ?, ?)", data)
    con.commit()


def create_cart(i, item, data=None):
    if data is None:
        data = [
            i, item
        ]
    print(data)
    con.execute(
        "INSERT INTO Cart (Id, Items)VALUES(?, ?)", data)
    con.commit()


def create_command(i, item, data=None):
    if data is None:
        data = [
            i, item
        ]
    print(data)
    con.execute(
        "INSERT INTO Command (Id, Items)VALUES(?, ?)", data)
    con.commit()


def create_commands(i, command, data=None):
    if data is None:
        data = [
            i, command
        ]
    print(data)
    con.execute(
        "INSERT INTO Commands (Id, Command)VALUES(?, ?)", data)
    con.commit()


def create_invoice(i, item, payment, data=None):
    if data is None:
        data = [
            i, item, Faker().date(), payment
        ]
    print(data)
    con.execute(
        "INSERT INTO Invoice (Id, Cart, Date, Payment)VALUES(?, ?, ?, ?)", data)
    con.commit()


def create_invoices(i, invoice, data=None):
    if data is None:
        data = [
            i, invoice
        ]
    print(data)
    con.execute(
        "INSERT INTO Invoices (Id, Invoice)VALUES(?, ?)", data)
    con.commit()


def create_login_info(i, data=None):
    if data is None:
        data = [
            i, Faker().email(), hash_passwd(Faker().password())
        ]
    print(data)
    con.execute(
        "INSERT INTO Login_info (Id, mail, Password)VALUES(?, ?, ?)", data)
    con.commit()


def create_payment(i, data=None):
    if data is None:
        data = [
            i, Faker().currency()[0]
        ]
    print(data)
    con.execute(
        "INSERT INTO Payment (Id, Payment)VALUES(?, ?)", data)
    con.commit()


def create_prefer_payment(i, data=None):
    if data is None:
        data = [
            i, Faker().random_int()
        ]
    print(data)
    con.execute(
        "INSERT INTO Prefer_payment (Id, Payment)VALUES(?, ?)", data)
    con.commit()


def create_rating(i, comment, data=None):
    if data is None:
        data = [
            i, random.randint(0, 5), comment
        ]
    print(data)
    con.execute(
        "INSERT INTO Rating (Id, Rating, Comment)VALUES(?, ?, ?)", data)
    con.commit()


def create_comment(i, user, data=None):
    if data is None:
        data = [
            i, Faker().text(), user
        ]
    print(data)
    con.execute(
        "INSERT INTO Comment (Id, Comment, User)VALUES(?, ?, ?)", data)
    con.commit()
