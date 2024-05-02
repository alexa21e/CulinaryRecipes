import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Author } from "../models/author";

@Injectable({
    providedIn: 'root'
})

export class AuthorsService {
    baseUrl = 'https://localhost:5001/api/Authors'
    
    constructor(private http: HttpClient) {
    }

    getMostProlificAuthors(authorsNumber: number){
        let params = new HttpParams();
        params = params.append('authorsNumber', authorsNumber);
        return this.http.get<Author[]>(this.baseUrl + '/mostProlific', {params});
    }
}