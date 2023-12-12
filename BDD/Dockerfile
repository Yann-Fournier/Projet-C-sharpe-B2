FROM albericwalsh/ecommerce:BDD-ecomerce
WORKDIR /app
RUN echo "BDD Dockerfile processing..."
COPY . /app
ADD script.sql /docker-entrypoint-initdb.d
EXPOSE 3306