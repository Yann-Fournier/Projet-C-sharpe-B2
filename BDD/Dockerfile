FROM albericwalsh/ecommerce:BDD-ecomerce
WORKDIR /app
RUN echo "BDD Dockerfile processing..."
COPY . /app
ADD script.sql /docker-entrypoint-initdb.d
# RUN mysql -u root -proot -p3306:3306 database < script.sql
EXPOSE 3306