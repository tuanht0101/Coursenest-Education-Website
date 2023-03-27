FROM node:19-alpine3.16 AS base
WORKDIR /app

FROM base AS build
COPY ["Web/user-app-react/package*.json", "./"]
RUN npm install
COPY ["Web/user-app-react/", "./"]
RUN npm run build

FROM base AS final
ENV NODE_ENV production
EXPOSE 5000
RUN npm install -g serve
COPY --from=build /app/build build/
CMD [ "serve", "build", "-l", "5000" ]