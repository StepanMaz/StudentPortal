FROM node:23 AS dev

WORKDIR /app

RUN npm install -g @angular/cli

COPY package*.json ./
RUN npm install --force

COPY . .

EXPOSE 3000
CMD ["ng", "serve", "--host", "0.0.0.0", "--port", "3000", "--disable-host-check"]


FROM node:23 AS build
WORKDIR /app

COPY package*.json ./
RUN npm install --force

COPY . .
RUN npm run build -- --configuration production


FROM nginx:alpine AS production

COPY --from=build /app/dist/ /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
