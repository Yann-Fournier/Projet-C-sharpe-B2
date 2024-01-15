FROM albericwalsh/ecommerce:BDD-ecomerce
WORKDIR /app
RUN echo "BDD Dockerfile processing..."
COPY . /app
#RUN chmod +x ./mariaDB_install.sh
#RUN ./mariaDB_install.sh
#RUN sqlcmd -S BDD -d database.sql -U root -P root -i ./script.sql | echo "script.sql failed"
#RUN mysql -u root -p1234 -h BDD < ./database.sql
ADD script.sql /docker-entrypoint-initdb.d
EXPOSE 3306
#ENTRYPOINT ["docker-entrypoint.sh"]
#CMD ["mysqld"]
#RUN chmod +x ./python_install.sh
#RUN ./python_install.sh
#RUN chmod +x ./Faker/main.py
#RUN ./Faker/main.py
