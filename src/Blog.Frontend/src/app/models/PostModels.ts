import { Tag } from "./TagModels";
import { CommentaryDTO } from "./CommentaryModels";
import { Like } from "./LikeModels";
export type PostList = {
    id: number;
    text: string;
    header: string;
    tags: Tag[];
    dateTime: string;
    commentariesCount: number;
    isDraft: boolean;
    likes: Like[];
}

export type PostsPage = {
    posts: PostList[];
    currentPage : number;
    pageCount: number;
    postsCount: number;
}

export type PostDTO = {
    id: number;
    text: string;
    header: string;
    tags: Tag[];
    dateTime: string;
    isDraft: boolean;
    likes: Like[];
}

export type PostSingleVM = {
    post: PostDTO;
    commentaries: CommentaryDTO[];  
    currentPage: number;
    pageCount: number;
}

export type CreatePostDTO = {
    text: string;
    header: string;
    tagString: string;
    isDraft: boolean;
}

export type UpdatePostDTO = {
    id: number;
    text: string;
    header: string;
    tagString: string;
    isDraft: boolean;
}

