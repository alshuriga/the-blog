import { environment } from "src/environments/environment";

export const url: string = environment.production ? "https://the-blog-api.alshuriga.ink/api/" : "https://localhost:80/api/"