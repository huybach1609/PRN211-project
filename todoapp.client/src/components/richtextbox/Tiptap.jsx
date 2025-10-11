import React, { useRef, useCallback, useEffect } from 'react';
import { useEditor, EditorContent } from '@tiptap/react';
import { Document } from '@tiptap/extension-document';
import { Paragraph } from '@tiptap/extension-paragraph';
import { Text } from '@tiptap/extension-text';
import { Bold } from '@tiptap/extension-bold';
import { Italic } from '@tiptap/extension-italic';
import { ListItem } from '@tiptap/extension-list-item';
import { BulletList } from '@tiptap/extension-bullet-list';
import { OrderedList } from '@tiptap/extension-ordered-list';
import { Button } from '@heroui/button';
import { BoldIcon, ItalicIcon, ListOrdered, Logs } from 'lucide-react';

const MenuBar = ({ editor }) => {
    if (!editor) {
        return null;
    }

    return (
        <div className="flex space-x-2 mb-2 p-2 rounded">
            <Button
                size='sm'
                onPress={() => editor.chain().focus().toggleBold().run()}
                className={`p-2 ${editor.isActive('bold') ? 'bg-blue-500' : 'bg-bg5'} rounded`}
                isIconOnly
            >
                <BoldIcon />
            </Button>
            <Button
                size='sm'
                onPress={() => editor.chain().focus().toggleItalic().run()}
                className={`p-2 ${editor.isActive('italic') ? 'bg-blue-500' : 'bg-bg5'} rounded`}
                isIconOnly
            >
                <ItalicIcon />
            </Button>
            <Button
                size='sm'
                onPress={() => editor.chain().focus().toggleBulletList().run()}
                className={`p-2 ${editor.isActive('bulletList') ? 'bg-blue-200' : 'bg-bg5'} rounded`}
                isIconOnly
            >
                <Logs />
            </Button>
            <Button
                size='sm'
                onPress={() => editor.chain().focus().toggleOrderedList().run()}
                className={`p-2 ${editor.isActive('orderedList') ? 'bg-blue-200' : 'bg-bg5'} rounded`}
                isIconOnly
            >
                <ListOrdered />
            </Button>
        </div>
    );
};

const Tiptap = ({ 
    onContentChange, 
    content ='Start typing your text here...' // Add a new prop to explicitly set content
}) => { 
    const editorRef = useRef(null); 
 
    const editor = useEditor({ 
        extensions: [ 
            Document, 
            Paragraph, 
            Text, 
            Bold, 
            Italic, 
            ListItem, 
            BulletList, 
            OrderedList 
        ], 
        content: content, 
        onUpdate: ({ editor }) => { 
            // Get HTML content 
            const htmlContent = editor.getHTML(); 
 
            // Call the prop function to pass content up to parent 
            onContentChange(htmlContent); 
        } 
    }); 
 
    // Add useEffect to update content when content prop changes
    useEffect(() => {
        if (editor && content) {
            editor.commands.setContent(content);
        }
    }, [content, editor]);
 
    // Create a method to get HTML content that can be called by parent 
    const getHTML = useCallback(() => { 
        return editor ? editor.getHTML() : ''; 
    }, [editor]); 
 
    // Optionally, attach the method to a ref if the parent needs to call it 
    React.useEffect(() => { 
        if (editorRef.current) { 
            editorRef.current.getHTML = getHTML; 
        } 
    }, [getHTML]); 
 
    if (!editor) { 
        return null; 
    } 
 
    return ( 
        <div className="border-bg5 border-[2px] rounded-xl p-2 my-3 h-[100%]"> 
            <MenuBar editor={editor} /> 
            <EditorContent 
                editor={editor} 
                className="p-2 outline-none focus:outline-none h-[70%] overflow-y-scroll" 
            /> 
        </div> 
    ); 
};
 


export default Tiptap;