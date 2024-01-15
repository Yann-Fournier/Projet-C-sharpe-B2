FROM albericwalsh/ecommerce:WEB-ecomerce
#COPY ./default.conf /etc/nginx/conf.d/default.conf
WORKDIR /docker-entrypoint.d
ADD dotnet_install.sh .
RUN chmod +x dotnet_install.sh
RUN ./dotnet_install.sh
COPY ./start.sh /
EXPOSE 8080
WORKDIR /app
COPY ./Test_Projet.csproj .
#RUN dotnet restore
COPY ./Program.cs .
#RUN dotnet publish -c Release -o out
#ENTRYPOINT ["dotnet", "out/Test_Projet.dll"]
CMD ["/start.sh"]