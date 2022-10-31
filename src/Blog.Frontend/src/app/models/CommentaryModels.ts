export type CommentaryDTO = {
    id: number;
    username: string;
    email: string;
    text: string;
    dateTime: string;
}

export type CreateCommenaryDTO = {
    text: string;
    postId: number;
}