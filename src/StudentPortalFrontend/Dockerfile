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

FROM node:23 as production

COPY --from=build /app /app

WORKDIR /app

EXPOSE 3000

CMD ["npm", "run", "serve:ssr:StudentPortalFrontend"]
