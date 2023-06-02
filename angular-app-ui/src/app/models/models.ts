//In this file we keep all the files of modules 
export interface SideNavItem{
title:string; //name of link in side nav
link:string; //cliked value
}

export enum UserType{
    ADMIN,
    USER,
}

//model for sending the data in backend
export interface User{
    id:number;
    firstName:string;
    lastName:string;
    email:string;
    mobile:number;
    password:string;
    blocked:boolean;
    active:boolean;
    createdOn:string;
    userType:UserType;
    fine:number;

}