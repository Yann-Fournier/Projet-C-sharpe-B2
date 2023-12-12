FROM albericwalsh/ecommerce:BDD-ecomerce
WORKDIR /app
COPY . /app
ADD script.sql /docker-entrypoint-initdb.d
EXPOSE 3306