import { create } from "zustand"

export const useCatStore = create((set) => ({
 selectedCatItem:{id:0,catImgName:"a",catScore:0},
 setCatItem: newCatItem => set({ selectedCatItem:newCatItem }),

}))