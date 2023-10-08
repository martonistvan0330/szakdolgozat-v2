FROM node:16-alpine AS build
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build --configuration=production

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=build app/dist/client-app/ .
EXPOSE 80
