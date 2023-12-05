import os
from random import random

from FakerHandler import *

produit = eval(input("Entrez un nombre de produit à générer: "))
user = eval(input("Entrez un nombre d'utilisateur à générer: "))
payment = eval(input("Entrez un nombre de type de paiement à générer: "))


def main():

    photo = 0
    category = 0
    rating = 0
    comment = 0
    login_info = 0
    address = 0
    commands = 0
    command = 0
    cart = 0
    invoicies = 0
    invoicie = 0

    for i in range(payment):
        create_payment(i)


    for i in range(produit):
        create_product(i, photo, category, random.randint(0, user), rating)

        # dependances
        create_photo(photo)
        create_category(category)
        create_rating(rating, comment)
        create_comment(comment, random.randint(0, user))

        photo += 1
        category += 1
        rating += 1
        comment += 1

    for i in range(user):
        create_user(i, login_info, address, photo, commands, cart, invoicies, random.randint(0, payment), rating)

        # dependances
        create_login_info(login_info)
        create_address(address)
        create_photo(photo)
        create_commands(commands, command)
        for j in range(random.randint(0, 5)):
            create_command(command, random.randint(0, produit))

        for j in range(random.randint(0, 5)):
            create_cart(cart, random.randint(0, produit))

        create_invoices(invoicies, invoicie)
        for j in range(random.randint(0, 5)):
            create_invoice(invoicie, random.randint(0, produit), random.randint(0, payment))

        create_rating(rating, comment)
        create_comment(comment, random.randint(0, user))

        login_info += 1
        address += 1
        photo += 1
        commands += 1
        cart += 1
        command += 1
        invoicies += 1
        invoicie += 1
        rating += 1
        comment += 1

main()